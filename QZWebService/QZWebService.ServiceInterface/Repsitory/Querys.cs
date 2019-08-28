using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace QZWebService.ServiceInterface.Repsitory
{
    public interface IPagingOptions
    {
        int PageSize { get; set; }
        int PageNumber { get; set; }
        int Skip { get; }
        int Take { get; }
        int TotalItems { get; set; }
    }

    internal enum SortType
    {
        SortByField,
        SortByIndex,
        SortByExp,
        NotSort
    }

    public class SortingOptions<T>
    {
        private readonly string orderBy;
        private readonly long columnIndex;
        private readonly Expression<Func<T, object>> keySelector;
        private readonly bool isDescending;
        private readonly SortType sortType;

        private string thenOrderBy;
        private Expression<Func<T, object>> thenKeySelector;
        private SortType thenSortType;
        private bool thenSortIsDescending;

        public SortingOptions()
        {
            this.thenOrderBy = string.Empty;
            sortType = SortType.NotSort;
            thenSortType = SortType.NotSort;
            thenSortIsDescending = false;
        }

        public SortingOptions(string sortProperty, bool isDescending = false) : this()
        {
            this.sortType = SortType.SortByField;
            this.isDescending = isDescending;
            this.orderBy = sortProperty;
        }

        public SortingOptions(Expression<Func<T, object>> keySelector, bool isDescending = false) : this()
        {
            this.sortType = SortType.SortByExp;
            this.isDescending = isDescending;
            this.keySelector = keySelector;
        }

        public SortingOptions(long columnIndex, bool isDescending = false) : this()
        {
            this.sortType = SortType.SortByExp;
            this.isDescending = isDescending;
            this.columnIndex = columnIndex;
        }

        public void ThenSortBy(string sortProperty, bool isDescending = false)
        {
            this.thenSortType = SortType.SortByField;
            this.thenOrderBy = sortProperty;
            this.thenSortIsDescending = isDescending;
        }

        public void ThenSortBy(Expression<Func<T, object>> keySelector, bool isDescending = false)
        {
            this.thenSortType = SortType.SortByExp;
            this.thenKeySelector = keySelector;
            this.thenSortIsDescending = isDescending;
        }

        public SqlExpression<T> BuildSortExp(SqlExpression<T> existExp)
        {
            var sortExp = existExp;
            if (sortType != SortType.NotSort)
            {
                if (isDescending)
                {
                    if (sortType == SortType.SortByExp)
                    {
                        sortExp = existExp.OrderByDescending<T>(keySelector);
                    }
                    else if (sortType == SortType.SortByField)
                    {
                        sortExp = existExp.OrderByDescending(orderBy);
                    }
                    else if (sortType == SortType.SortByIndex)
                    {
                        sortExp = existExp.OrderByDescending(columnIndex);
                    }

                }
                else
                {
                    if (sortType == SortType.SortByExp)
                    {
                        sortExp = existExp.OrderBy<T>(keySelector);
                    }
                    else if (sortType == SortType.SortByField)
                    {
                        sortExp = existExp.OrderBy(orderBy);
                    }
                    else if (sortType == SortType.SortByIndex)
                    {
                        sortExp = existExp.OrderBy(columnIndex);
                    }
                }
            }

            if (thenSortType != SortType.NotSort)
            {
                if (isDescending)
                {
                    if (thenSortType == SortType.SortByExp)
                    {
                        sortExp = sortExp.ThenByDescending<T>(keySelector);
                    }
                    else if (thenSortType == SortType.SortByField)
                    {
                        sortExp = sortExp.ThenByDescending(orderBy);
                    }
                }
                else
                {
                    if (sortType == SortType.SortByExp)
                    {
                        sortExp = sortExp.ThenBy<T>(keySelector);
                    }
                    else if (sortType == SortType.SortByField)
                    {
                        sortExp = sortExp.ThenBy(orderBy);
                    }
                }
            }

            return sortExp;
        }

        public override string ToString()
        {
            var val = String.Format("SortingOptions<{0}>\nSort:\nExtra: {1}",
                (typeof(T)).Name,
                isDescending
                );
            return val;
        }
    }


    public class PagingOptions<T> : SortingOptions<T>
    {

        public int Skip { get; set; }
        public int Take { get; set; }
        public int TotalItems { get; set; }

        public PagingOptions(int skip, int take, string sortProperty, bool isDescending = false)
            : base(sortProperty, isDescending)
        {
            this.Skip = skip;
            this.Take = take;
        }

        public PagingOptions(int skip, int take, Expression<Func<T, object>> keySelector, bool isDescending = false)
            : base(keySelector, isDescending)
        {
            this.Skip = skip;
            this.Take = take;
        }

        public PagingOptions(int skip, int take, long columnIndex, bool isDescending = false)
            : base(columnIndex, isDescending)
        {
            this.Skip = skip;
            this.Take = take;
        }


        /// <summary>
        /// Used in compiling a unique key for a query
        /// </summary>
        /// <returns>Unique key for a query</returns>
        public override string ToString()
        {
            return String.Format("PagingOptions<{0}>\nTake: {1}\nSkip: {2}\nSort: {3}",
                (typeof(T)).Name,
                Take,
                Skip,
                base.ToString()
                );
        }
    }
}
