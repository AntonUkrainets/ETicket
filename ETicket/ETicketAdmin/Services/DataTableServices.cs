﻿using ETicket.Admin.Extensions;
using ETicket.Admin.Models.DataTables;
using ETicket.Admin.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ETicket.Admin.Services
{
    public class DataTableServices<T>
    {
        private readonly IDataTablePaging<T> service;
        public DataTableServices(IDataTablePaging<T> service)
        {
            this.service = service;
        }

        public object GetDataTablePage(DataTablePagingInfo pagingInfo)
        {
            var data = service.GetAll();
            var drawStep = pagingInfo.DrawCounter;
            var countRecords = pagingInfo.TotalEntries;

            //For single count query
            if (countRecords == -1)
            {
                countRecords = data.Count();
            }

            data = GetSortedQuery(data, pagingInfo.SortingColumnNumber
                , pagingInfo.SortingColumnDirection, service.GetSortExpression());

            var countFiltered = countRecords;
            string searchString = pagingInfo.SearchValue;

            if (!string.IsNullOrEmpty(searchString))
            {
                data = GetSearchedQuery(data, service.GetSearchExpressions(searchString));
                countFiltered = data.Count();
            }

            data = data
                    .Skip((pagingInfo.PageNumber - 1) * pagingInfo.PageSize)
                    .Take(pagingInfo.PageSize);

            return GetJsonDataTable(data, drawStep, countRecords, countFiltered);
        }

        private object GetJsonDataTable(IQueryable<T> data, int drawCounter, int countRecords, int countFiltered)
        {
            return new
            {
                draw = ++drawCounter,
                recordsTotal = countRecords,
                recordsFiltered = countFiltered,
                data = data
            };
        }

        private IQueryable<T> GetSortedQuery(IQueryable<T> query, int columnNumber, string columnDirection, IList<Expression<Func<T, string>>> expressions)
        {
            return query.ApplySortBy(expressions[columnNumber], columnDirection);
        }

        private IQueryable<T> GetSearchedQuery(IQueryable<T> query, Expression<Func<T, bool>> expression)
        {
            return query.Where(expression);
        }

        private IQueryable<T> GetSearchedQuery(IQueryable<T> query, IList<Expression<Func<T, bool>>> expressions)
        {
            var exp = expressions.Combine();

            return query.Where(exp);
        }
    }
}
