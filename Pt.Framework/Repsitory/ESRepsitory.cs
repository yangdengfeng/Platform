using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using Elasticsearch.Net;

namespace Pkpm.Framework.Repsitory
{
    public class ESRepsitory<Entity> : IESRepsitory<Entity>
        where Entity : class
    {
        IConnectionSettingsValues connctionSettings;

        public ESRepsitory(IConnectionSettingsValues connctionSettings)
        {
            this.connctionSettings = connctionSettings;
        }

        public IGetResponse<Entity> Get(DocumentPath<Entity> document)
        {
            var client = new ElasticClient(connctionSettings); 

            return client.Get<Entity>(document);
        }

        public IIndexResponse Index(Entity document, Func<IndexDescriptor<Entity>, IIndexRequest> selector)
        {
            var client = new ElasticClient(connctionSettings);

            return client.Index(document, selector);
        }

        public ISearchResponse<Entity> Search(Func<SearchDescriptor<Entity>, ISearchRequest> selector)
        {
            var client = new ElasticClient(connctionSettings);

            return client.Search<Entity>(selector);
        }

        public IUpdateResponse<Entity> Update<TPartialDocument>(DocumentPath<Entity> documentPath, Func<UpdateDescriptor<Entity, TPartialDocument>, IUpdateRequest<Entity, TPartialDocument>> selector) where TPartialDocument : class
        {
            var client = new ElasticClient(connctionSettings);

            return client.Update<Entity, TPartialDocument>(documentPath, selector);
        }

        public IDeleteResponse Delete<TPartialDocument>(DocumentPath<Entity> documentPath,Func<DeleteDescriptor<Entity>,IDeleteRequest> selector)
        {
            var client = new ElasticClient(connctionSettings);
            return client.Delete<Entity>(documentPath,selector);
        }

        public IBulkResponse Bulk(IBulkRequest request)
        {
            var client = new ElasticClient(connctionSettings);

            return client.Bulk(request);
        }

        public IUpdateByQueryResponse UpdateByQuery(Func<UpdateByQueryDescriptor<Entity>, IUpdateByQueryRequest> selector)
        {
            var client = new ElasticClient(connctionSettings);

            return client.UpdateByQuery<Entity>(selector);
        }

        public ICountResponse Count(Func<CountDescriptor<Entity>, ICountRequest> selector)
        {
            var client = new ElasticClient(connctionSettings);

            return client.Count(selector);
        }
    }

    public static class ESConnectionSettings
    {
        private static Lazy<IConnectionSettingsValues> connectionSettingHolder = new Lazy<IConnectionSettingsValues>(() =>
        {
            //统一的设置入口
            string esNodeUrl = System.Configuration.ConfigurationManager.AppSettings["ESUrl"];
            if(esNodeUrl.Contains(","))
            {
                var uris = esNodeUrl.Split(',').Select(es => new Uri(es)).ToList(); 

                var connectionPool = new SniffingConnectionPool(uris);
                ConnectionSettings settings = new ConnectionSettings(connectionPool).DefaultIndex("gx-tbpitem").DefaultTypeNameInferrer(ft => ft.Name.ToString()).DefaultFieldNameInferrer(f => f);
                return settings;
            }
            else
            {
                Uri node = new Uri(esNodeUrl);
                ConnectionSettings settings = new ConnectionSettings(node).DefaultIndex("gx-tbpitem").DefaultTypeNameInferrer(ft => ft.Name.ToString()).DefaultFieldNameInferrer(f => f);

                return settings;
            }
           
        });

        public static IConnectionSettingsValues connectionSettings
        {
            get
            {
                return connectionSettingHolder.Value;
            }
        }

    }
}
