using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Framework.Repsitory
{
    public interface IESRepsitory<Entity>
         where Entity : class
    {
        /// <summary>
        /// 通过NEST的Search api进行查询
        /// </summary>
        /// <param name="selector">selector</param>
        /// <returns>查询结果</returns>
        ISearchResponse<Entity> Search(Func<SearchDescriptor<Entity>, ISearchRequest> selector);

        /// <summary>
        /// 直接通过ID进行查询
        /// </summary>
        /// <param name="document">包含ID的文档</param>
        /// <returns>查询结果</returns>
        IGetResponse<Entity> Get(DocumentPath<Entity> document);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        /// <param name=""></param>
        /// <returns></returns>
        IIndexResponse Index(Entity document, Func<IndexDescriptor<Entity>, IIndexRequest> selector);


        IUpdateResponse<Entity> Update<TPartialDocument>(DocumentPath<Entity> documentPath, Func<UpdateDescriptor<Entity, TPartialDocument>, IUpdateRequest<Entity, TPartialDocument>> selector) where TPartialDocument : class;

        IBulkResponse Bulk(IBulkRequest request);

        //IUpdateByQueryResponse UpdateByQuery(IUpdateByQueryRequest request);

        IUpdateByQueryResponse UpdateByQuery(Func<UpdateByQueryDescriptor<Entity>, IUpdateByQueryRequest> selector);

        IDeleteResponse Delete<TPartialDocument>(DocumentPath<Entity> documentPath, Func<DeleteDescriptor<Entity>, IDeleteRequest> selector);

        ICountResponse Count(Func<CountDescriptor<Entity>, ICountRequest> selector);
    }
}
