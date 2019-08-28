using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QZWebService.ServiceInterface.Repsitory
{
    public class ServiceStackRepsitory<Entity> : IRepsitory<Entity>
      where Entity : class
    {
        IDbConnectionFactory dbFactory;

        public ServiceStackRepsitory(IDbConnectionFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        #region 聚合操作
        public long GetCountByCondtion(Expression<Func<Entity, bool>> preditate)
        {
            using (var db = dbFactory.Open())
            {


                return db.Count(preditate);
            }
        }

        public TResult GetScalar<TResult>(Expression<Func<Entity, object>> keySelector, Expression<Func<Entity, bool>> predicate)
        {
            using (var db = dbFactory.Open())
            {
                return db.Scalar<Entity, TResult>(keySelector, predicate);
            }
        }


        #endregion

        #region Read Operation

        public Dictionary<TKey, TValue> GetDictByGroupBy<TKey, TValue>(Expression<Func<Entity, object>> keySelector,
           Expression<Func<Entity, bool>> predicate,
           Expression<Func<Entity, bool>> havingPredicate,
           Expression<Func<Entity, object>> selector)
        {
            using (var db = dbFactory.Open())
            {
                var sqlExp = db.From<Entity>();

                if (predicate != null)
                {
                    sqlExp = sqlExp.Where(predicate);
                }

                sqlExp = sqlExp.GroupBy(keySelector);

                if (havingPredicate != null)
                {
                    sqlExp = sqlExp.Having(havingPredicate);
                }

                return db.Dictionary<TKey, TValue>(sqlExp.Select(selector));
            }
        }


        public Entity GetById(object Id)
        {
            using (var db = dbFactory.Open())
            {
                return db.SingleById<Entity>(Id);
            }
        }

        public Entity GetSingleByCondition(Expression<Func<Entity, bool>> predicate)
        {
            using (var db = dbFactory.Open())
            {
                var sqlExp = db.From<Entity>();

                if (predicate != null)
                {
                    sqlExp = sqlExp.Where(predicate);
                }

                return db.Select(sqlExp.Limit(0, 1)).FirstOrDefault();
            }
        }

        public TResult GetSingleByCondition<TResult>(Expression<Func<Entity, bool>> predicate, Expression<Func<Entity, object>> keySelector)
        {
            using (var db = dbFactory.Open())
            {
                var sqlExp = db.From<Entity>();

                if (predicate != null)
                {
                    sqlExp = sqlExp.Where(predicate);
                }

                return db.Select<TResult>(sqlExp.Select(keySelector).Limit(0, 1)).FirstOrDefault();
            }
        }


        public List<Entity> GetByCondition(Expression<Func<Entity, bool>> predicate)
        {
            using (var db = dbFactory.Open())
            {
                var sqlExp = db.From<Entity>();
                if (predicate != null)
                {
                    sqlExp = sqlExp.Where(predicate);
                }

                return db.Select<Entity>(sqlExp);
            }
        }

        public List<TResult> GetByConditon<TResult>(Expression<Func<Entity, bool>> predicate, Expression<Func<Entity, object>> selector)
        {
            using (var db = dbFactory.Open())
            {
                var sqlExp = db.From<Entity>();
                if (predicate != null)
                {
                    sqlExp = sqlExp.Where(predicate);
                }

                return db.Select<TResult>(sqlExp.Select(selector));
            }
        }

        public List<Entity> GetByConditionSort(Expression<Func<Entity, bool>> predicate,
            SortingOptions<Entity> sortOption)
        {
            using (var db = dbFactory.Open())
            {
                var sqlExp = db.From<Entity>();

                if (predicate != null)
                {
                    sqlExp = sqlExp.Where(predicate);
                }

                if (sortOption != null)
                {
                    sqlExp = sortOption.BuildSortExp(sqlExp);
                }

                return db.Select<Entity>(sqlExp);

            }
        }

        public List<TResult> GetByConditionSort<TResult>(Expression<Func<Entity, bool>> predicate,
             Expression<Func<Entity, object>> selector,
             SortingOptions<Entity> sortOption)
        {
            using (var db = dbFactory.Open())
            {
                var sqlExp = db.From<Entity>();

                if (predicate != null)
                {
                    sqlExp = sqlExp.Where(predicate);
                }

                if (sortOption != null)
                {
                    sqlExp = sortOption.BuildSortExp(sqlExp);
                }
                return db.Select<TResult>(sqlExp.Select(selector));

            }
        }



        public List<Entity> GetByConditonPage(Expression<Func<Entity, bool>> predicate,
           PagingOptions<Entity> pagingOption)
        {
            using (var db = dbFactory.Open())
            {
                var sqlExp = db.From<Entity>();

                if (predicate != null)
                {
                    sqlExp = sqlExp.Where(predicate);
                }

                if (pagingOption != null)
                {
                    pagingOption.TotalItems = (int)db.Count(sqlExp);

                    sqlExp = pagingOption.BuildSortExp(sqlExp);
                    sqlExp = sqlExp.Limit(pagingOption.Skip, pagingOption.Take);
                }

                return db.Select<Entity>(sqlExp);
            }
        }

        public List<TResult> GetByConditonPage<TResult>(Expression<Func<Entity, bool>> predicate,
            Expression<Func<Entity, object>> selector,
            PagingOptions<Entity> pagingOption)
        {
            using (var db = dbFactory.Open())
            {
                var sqlExp = db.From<Entity>();

                if (predicate != null)
                {
                    sqlExp = sqlExp.Where(predicate);
                }

                if (pagingOption != null)
                {
                    pagingOption.TotalItems = (int)db.Count(sqlExp);

                    sqlExp = pagingOption.BuildSortExp(sqlExp);
                    sqlExp = sqlExp.Limit(pagingOption.Skip, pagingOption.Take);
                }

                return db.Select<TResult>(sqlExp.Select(selector));
            }
        }


        public Dictionary<TKey, TValue> GetDictByCondition<TKey, TValue>(Expression<Func<Entity, bool>> predicate,
            Expression<Func<Entity, object>> fields)
        {
            using (var db = dbFactory.Open())
            {
                var sqlExp = db.From<Entity>();

                if (predicate != null)
                {
                    sqlExp = sqlExp.Where(predicate);
                }

                return db.Dictionary<TKey, TValue>(sqlExp.Select(fields));
            }
        }

        public Dictionary<TKey, TValue> GetDictByConditionSort<TKey, TValue>(Expression<Func<Entity, bool>> predicate,
            Expression<Func<Entity, object>> fields,
            SortingOptions<Entity> sortOption)
        {
            using (var db = dbFactory.Open())
            {

                var sqlExp = db.From<Entity>();

                if (predicate != null)
                {
                    sqlExp = sqlExp.Where(predicate);
                }


                if (sortOption != null)
                {
                    sqlExp = sortOption.BuildSortExp(sqlExp);
                }

                return db.Dictionary<TKey, TValue>(sqlExp.Select(fields));
            }
        }

        public Dictionary<TKey, TValue> GetDictByConditionPage<TKey, TValue>(Expression<Func<Entity, bool>> predicate,
            Expression<Func<Entity, object>> fields,
            PagingOptions<Entity> pagingOption)
        {
            using (var db = dbFactory.Open())
            {
                var sqlExp = db.From<Entity>();

                if (predicate != null)
                {
                    sqlExp = sqlExp.Where(predicate);
                }
                if (pagingOption != null)
                {
                    sqlExp = pagingOption.BuildSortExp(sqlExp);
                    sqlExp = sqlExp.Limit(pagingOption.Skip, pagingOption.Take);
                }

                return db.Dictionary<TKey, TValue>(sqlExp.Select(fields));
            }
        }

        public Dictionary<TKey, List<TValue>> GetLookupDictByCondition<TKey, TValue>(Expression<Func<Entity, bool>> predicate,
           Expression<Func<Entity, object>> fields)
        {
            using (var db = dbFactory.Open())
            {
                var sqlExp = db.From<Entity>();

                if (predicate != null)
                {
                    sqlExp = sqlExp.Where(predicate);
                }

                return db.Lookup<TKey, TValue>(sqlExp.Select(fields));
            }
        }

        public List<T> GetColumnByCondition<T>(Expression<Func<Entity, bool>> predicate,
            Expression<Func<Entity, object>> fields)
        {
            using (var db = dbFactory.Open())
            {
                var sqlExp = db.From<Entity>();

                if (predicate != null)
                {
                    sqlExp = sqlExp.Where(predicate);
                }


                return db.Column<T>(sqlExp.Select(fields));
            }
        }

        public HashSet<T> GetColumnDistCondition<T>(Expression<Func<Entity, bool>> predicate,
            Expression<Func<Entity, object>> fields)
        {
            using (var db = dbFactory.Open())
            {
                var sqlExp = db.From<Entity>();

                if (predicate != null)
                {
                    sqlExp = sqlExp.Where(predicate);
                }


                return db.ColumnDistinct<T>(sqlExp.Select(fields));
            }
        }

        public long GetByMasterDetailCount<Detail>(Expression<Func<Entity, bool>> masterPredicate,
            Expression<Func<Detail, bool>> detailPredicate)
        {
            using (var db = dbFactory.Open())
            {
                var sqlExp = db.From<Entity>()
                    .Join<Detail>();

                if (masterPredicate != null)
                {
                    sqlExp = sqlExp.Where(masterPredicate);
                }

                if (detailPredicate != null)
                {
                    sqlExp = sqlExp.Where(detailPredicate);
                }

                return db.Count(sqlExp.SelectDistinct());
            }
        }

        public List<Entity> GetMasterDetailCondition<Detail>(Expression<Func<Entity, bool>> masterPredicate,
           Expression<Func<Detail, bool>> detailPredicate)
        {
            using (var db = dbFactory.Open())
            {
                var sqlExp = db.From<Entity>()
                    .Join<Detail>();

                if (masterPredicate != null)
                {
                    sqlExp = sqlExp.Where(masterPredicate);
                }

                if (detailPredicate != null)
                {
                    sqlExp = sqlExp.Where<Detail>(detailPredicate);
                }
                return db.Select(sqlExp.SelectDistinct());
            }
        }

        public List<Entity> GetMasterDetailConditionSort<Detail>(Expression<Func<Entity, bool>> masterPredicate,
           Expression<Func<Detail, bool>> detailPredicate,
            SortingOptions<Entity> sortOption)
        {
            using (var db = dbFactory.Open())
            {
                var sqlExp = db.From<Entity>()
                    .Join<Detail>();

                if (masterPredicate != null)
                {
                    sqlExp = sqlExp.Where(masterPredicate);
                }

                if (detailPredicate != null)
                {
                    sqlExp = sqlExp.Where<Detail>(detailPredicate);
                }

                if (sortOption != null)
                {
                    sqlExp = sortOption.BuildSortExp(sqlExp);
                }


                return db.Select(sqlExp.SelectDistinct());
            }
        }

        public long GetByJoinTableCount<Detail, Result>(Expression<Func<Entity, bool>> masterPredicate,
           Expression<Func<Detail, bool>> detailPredicate,
           Expression<Func<Entity, Detail, bool>> joinExp)
        {
            using (var db = dbFactory.Open())
            {
                var sqlExp = db.From<Entity>()
                    .Join<Detail>(joinExp);

                if (masterPredicate != null)
                {
                    sqlExp = sqlExp.Where(masterPredicate);
                }

                if (detailPredicate != null)
                {
                    sqlExp = sqlExp.Where<Detail>(detailPredicate);
                }

                return db.Count(sqlExp);

            }
        }

        public List<Result> GetByJoinTable<Detail, Result>(Expression<Func<Entity, bool>> masterPredicate,
            Expression<Func<Detail, bool>> detailPredicate,
            Expression<Func<Entity, Detail, bool>> joinExp,
            Expression<Func<Entity, Detail, object>> fields,
            Expression<Func<Entity, object>> orderFields,
            Expression<Func<Entity, object>> orderDescFields,
             int skip, int rows)
        {
            using (var db = dbFactory.Open())
            {
                var sqlExp = db.From<Entity>()
                    .Join<Detail>(joinExp);

                if (masterPredicate != null)
                {
                    sqlExp = sqlExp.Where(masterPredicate);
                }

                if (detailPredicate != null)
                {
                    sqlExp = sqlExp.Where<Detail>(detailPredicate);
                }

                if (orderFields != null)
                {
                    sqlExp = sqlExp.OrderBy(orderFields);
                }

                if (orderDescFields != null)
                {
                    sqlExp = sqlExp.OrderByDescending(orderDescFields);
                }

                return db.Select<Result>(sqlExp.Select<Entity, Detail>(fields).Limit(skip, rows));

            }
        }

        #endregion

        #region Insert Operation
        public long Insert(Entity entity)
        {
            using (var db = dbFactory.Open())
            {


                return db.Insert<Entity>(entity);


            }
        }

        public void InsertAll(List<Entity> entities)
        {
            using (var db = dbFactory.Open())
            {
                db.InsertAll(entities);
            }
        }
        #endregion

        #region Delete Operation

        public int DeleteById(object Id)
        {
            using (var db = dbFactory.Open())
            {
                return db.DeleteById<Entity>(Id);
            }
        }

        public Int32 DeleteByCondition(Expression<Func<Entity, bool>> predicate)
        {
            using (var db = dbFactory.Open())
            {
                return db.Delete(predicate);
            }
        }
        #endregion

        public int Update(Entity entity)
        {
            using (var db = dbFactory.Open())
            {
                return db.Update(entity);
            }
        }

        public int UpdateOnly(Entity entity, Expression<Func<Entity, bool>> predicate, Expression<Func<Entity, object>> fields)
        {
            using (var db = dbFactory.Open())
            {
                SqlExpression<Entity> sqlExp = db.From<Entity>().Where(predicate).Update(fields);
                return db.UpdateOnly<Entity>(entity, onlyFields: sqlExp);
            }
        }


    }
}
