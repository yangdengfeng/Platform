 
using ServiceStack.Model;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Framework.Repsitory
{
    public interface IRepsitory<Entity> 
        where Entity : class
    {
        /// <summary>
        /// 通过id获取实体
        /// </summary>
        /// <param name="Id">id</param>
        /// <returns></returns>
        Entity GetById(object Id);

        /// <summary>
        /// 通过查询条件获取一条记录
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        Entity GetSingleByCondition(Expression<Func<Entity, bool>> predicate);

        /// <summary>
        /// 通过查询条件获取一条记录的部分数据
        /// </summary>
        /// <typeparam name="TResult">部分数据实体</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="keySelector">其他类型的选择</param>
        /// <param name=""></param>
        /// <returns></returns>
        TResult GetSingleByCondition<TResult>(Expression<Func<Entity, bool>> predicate, Expression<Func<Entity, object>> keySelector);

        /// <summary>
        /// 通告查询条件获取数据结构条数
        /// </summary>
        /// <param name="preditate">简单查询条件</param>
        /// <returns></returns>
        long GetCountByCondtion(Expression<Func<Entity, bool>> preditate);

        /// <summary>
        /// 聚合操作
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="keySelector">聚合的一些操作如 x => Sql.Max(x.Age) </param>
        /// <param name="predicate">过滤条件</param>
        /// <returns></returns>
        TResult GetScalar<TResult>(Expression<Func<Entity, object>> keySelector, Expression<Func<Entity, bool>> predicate);

        /// <summary>
        /// 通过查询条件获取实体列表
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        List<Entity> GetByCondition(Expression<Func<Entity, bool>> predicate);

        /// <summary>
        /// 通过查询条件获取实体部分字段
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="selector">实体部分字段</param>
        /// <returns></returns>
        List<TResult> GetByConditon<TResult>(Expression<Func<Entity, bool>> predicate,
           Expression<Func<Entity, object>> selector);

        /// <summary>
        /// 通过查询条件排序获取分页数据
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="orders">升序</param>
        /// <param name="orderDesc">降序</param>
        /// <param name="skip"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        List<Entity> GetByCondition(Expression<Func<Entity, bool>> predicate,
           Expression<Func<Entity, object>> orders,
           Expression<Func<Entity, object>> orderDesc,
           int skip,
           int rows);

        /// <summary>
        /// 通过查询条件并排序获取
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="sortOption">排序</param>
        /// <returns></returns>
        List<Entity> GetByConditionSort(Expression<Func<Entity, bool>> predicate,
            SortingOptions<Entity> sortOption); 

        /// <summary>
        /// 通过查询条件获取
        /// </summary>
        /// <typeparam name="TResult">返回的其他类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="selector">其他类型的选择</param>
        /// <param name="sortOption">排序</param>
        /// <returns></returns>
        List<TResult> GetByConditionSort<TResult>(Expression<Func<Entity, bool>> predicate,
            Expression<Func<Entity, object>> selector,
            SortingOptions<Entity> sortOption);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <param name="pagingOption">分页</param>
        /// <returns></returns>
        List<Entity> GetByConditonPage(Expression<Func<Entity, bool>> predicate,
           PagingOptions<Entity> pagingOption);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="TResult">返回的其他类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="selector">其他类型的选择</param>
        /// <param name="pagingOption">分页</param>
        /// <returns></returns>
        List<TResult> GetByConditonPage<TResult>(Expression<Func<Entity, bool>> predicate,
            Expression<Func<Entity, object>> selector,
            PagingOptions<Entity> pagingOption);


        /// <summary>
        /// Groupby操作
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="keySelector">groupby 比如： x => x.ShipperTypeId</param>
        /// <param name="predicate">查询的条件 比如： x=> x.Age> 50 </param>
        /// <param name="havingPredicate">having的条件 比如： x=> Sql.Max(x.Age)>100 </param>
        /// <param name="selector"> 最后返回的两列 比如: x=> new { x.ShipperTypeId, Total=Sql.Count('*') </param>
        /// <returns></returns>
        Dictionary<TKey, TValue> GetDictByGroupBy<TKey, TValue>(Expression<Func<Entity, object>> keySelector,
            Expression<Func<Entity, bool>> predicate,
            Expression<Func<Entity, bool>> havingPredicate,
            Expression<Func<Entity, object>> selector); 

        /// <summary>
        /// 通过条件过滤获取Dictionary
        /// </summary>
        /// <typeparam name="TKey">key类型</typeparam>
        /// <typeparam name="TValue">value类型</typeparam>
        /// <param name="predicates">查询条件</param>
        /// <param name="fields">返回dictionary对于的字段如：s=>new{s.Id, s.Name}</param>
        /// <returns></returns>
        Dictionary<TKey, TValue> GetDictByCondition<TKey, TValue>(Expression<Func<Entity, bool>> predicate,
            Expression<Func<Entity, object>> fields);

        /// <summary>
        /// 通过条件过滤获取Dictionary
        /// </summary>
        /// <typeparam name="TKey">key类型</typeparam>
        /// <typeparam name="TValue">value类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="fields">返回dictionary对于的字段如：s=>new{s.Id, s.Name}</param>
        /// <param name="sortOption">排序</param>
        /// <returns></returns>
        Dictionary<TKey, TValue> GetDictByConditionSort<TKey, TValue>(Expression<Func<Entity, bool>> predicate,
            Expression<Func<Entity, object>> fields, 
            SortingOptions<Entity> sortOption);

        /// <summary>
        /// 通过条件过滤获取Dictionary
        /// </summary>
        /// <typeparam name="TKey">key类型</typeparam>
        /// <typeparam name="TValue">value类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="fields">返回dictionary对于的字段如：s=>new{s.Id, s.Name}</param>
        /// <param name="pagingOption">分页</param>
        /// <returns></returns>
        Dictionary<TKey, TValue> GetDictByConditionPage<TKey, TValue>(Expression<Func<Entity, bool>> predicate,
            Expression<Func<Entity, object>> fields,
            PagingOptions<Entity> pagingOption);


        /// <summary>
        /// 通过条件过滤
        /// </summary>
        /// <typeparam name="TKey">key类型</typeparam>
        /// <typeparam name="TValue">value类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="fields">返回对于的字段如　：s=>new{s.Id, s.Name}</param>
        /// <returns></returns>
        Dictionary<TKey, List<TValue>> GetLookupDictByCondition<TKey, TValue>(Expression<Func<Entity, bool>> predicate,
           Expression<Func<Entity, object>> fields);

        /// <summary>
        /// 通过条件发挥某一列的集合
        /// </summary>
        /// <typeparam name="T">一列的类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="fields">列</param>
        /// <returns></returns>
        List<T> GetColumnByCondition<T>(Expression<Func<Entity, bool>> predicate,
             Expression<Func<Entity, object>> fields);

        /// <summary>
        /// 通过条件发挥某一列distinct集合
        /// </summary>
        /// <typeparam name="T">一列的类型</typeparam>
        /// <param name="predicate">查询条件</param>
        /// <param name="fields">列</param>
        /// <returns></returns>
        HashSet<T> GetColumnDistCondition<T>(Expression<Func<Entity, bool>> predicate,
           Expression<Func<Entity, object>> fields);

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <typeparam name="Detail"></typeparam>
        /// <param name="masterPredicate"></param>
        /// <param name="detailPredicate"></param>
        /// <returns></returns>
        long GetByMasterDetailCount<Detail>(Expression<Func<Entity, bool>> masterPredicate,
            Expression<Func<Detail, bool>> detailPredicate);

        /// <summary>
        /// 根据主从表获取实体数据
        /// </summary>
        /// <typeparam name="Detail">从表</typeparam>
        /// <param name="masterPredicate">主表查询条件</param>
        /// <param name="detailPredicate">从表查询条件</param>  
        /// <returns></returns>
        List<Entity> GetMasterDetailCondition<Detail>(Expression<Func<Entity, bool>> masterPredicate,
            Expression<Func<Detail, bool>> detailPredicate);

        /// <summary>
        /// 根据主从表获取实体数据
        /// </summary>
        /// <typeparam name="Detail">从表</typeparam>
        /// <param name="masterPredicate">主表查询条件</param>
        /// <param name="detailPredicate">从表查询条件</param> 
        /// <param name="sortOption">排序</param>
        /// <returns></returns>
        List<Entity> GetMasterDetailConditionSort<Detail>(Expression<Func<Entity, bool>> masterPredicate,
            Expression<Func<Detail, bool>> detailPredicate, 
            SortingOptions<Entity> sortOption);

        /// <summary>
        /// 获取条数
        /// </summary>
        /// <typeparam name="Detail"></typeparam>
        /// <typeparam name="Result"></typeparam>
        /// <param name="masterPredicate"></param>
        /// <param name="detailPredicate"></param>
        /// <param name="joinExp"></param>
        /// <returns></returns>
        long GetByJoinTableCount<Detail, Result>(Expression<Func<Entity, bool>> masterPredicate,
           Expression<Func<Detail, bool>> detailPredicate,
           Expression<Func<Entity, Detail, bool>> joinExp);

        /// <summary>
        /// 主表和从表join返回新的实体
        /// </summary>
        /// <typeparam name="Detail">从表</typeparam>
        /// <typeparam name="Result">新的实体</typeparam>
        /// <param name="masterPredicate">主表查询条件</param>
        /// <param name="detailPredicate">从表查询条件</param>
        /// <param name="joinExp">join条件</param>
        /// <param name="fields">返回新实体的字段</param>
        /// <param name="orderFields">排序字段</param>
        /// <param name="orderDescFields">降序字段</param>
        /// <param name="skip">skip行数</param>
        /// <param name="rows">行数</param>
        /// <returns></returns>
        List<Result> GetByJoinTable<Detail, Result>(Expression<Func<Entity, bool>> masterPredicate,
            Expression<Func<Detail, bool>> detailPredicate,
            Expression<Func<Entity, Detail, bool>> joinExp,
            Expression<Func<Entity, Detail, object>> fields,
            Expression<Func<Entity, object>> orderFields,
            Expression<Func<Entity, object>> orderDescFields,
             int skip, int rows);

        /// <summary>
        /// 插入对应的实体到数据库
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        Int64 Insert(Entity entity);

        /// <summary>
        /// 插入多条实体到数据库
        /// </summary>
        /// <param name="entities">多条实体</param>
        void InsertAll(List<Entity> entities);

        /// <summary>
        /// 更新实体到数据库
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        Int32 Update(Entity entity);

        /// <summary>
        /// 更新实体的部分属性到数据库
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="predicate">更新的过滤条件，一般是通过id过滤</param>
        /// <param name="fields">要部分更新的实体属性</param>
        /// <returns></returns>
        Int32 UpdateOnly(Entity entity,Expression<Func<Entity,bool>> predicate, Expression<Func<Entity, object>> fields);

       /// <summary>
       /// 通过id删除
       /// </summary>
       /// <param name="Id">Id</param>
       /// <returns></returns>
        Int32 DeleteById(object Id);

        /// <summary>
        /// 通过查询条件删除
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        Int32 DeleteByCondition(Expression<Func<Entity, bool>> predicate);

    }
}
