using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace _360LawGroup.CostOfSalesBilling.Data.Common
{
    public enum WhereOperation { Equal = 0, NotEqual = 1, Contains = 2, GretterEq = 3, Gretter = 4, Less = 5, LessEq = 6, Like = 7 }
    // Helper class that captures most of the change tracking work that needs to be done
    // for self tracking entities.
    public static class DbExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> items, string propertyName)
        {
            try
            {
                var typeOfT = typeof(T);
                var parameter = Expression.Parameter(typeOfT, "parameter");
                var prop = typeOfT.GetProperties().Where(p => p.Name.ToLower() == propertyName.ToLower()).FirstOrDefault();
                propertyName = prop.Name;
                var propertyType = prop.PropertyType;
                var propertyAccess = Expression.PropertyOrField(parameter, propertyName);
                var orderExpression = Expression.Lambda(propertyAccess, parameter);

                var expression = Expression.Call(typeof(Queryable), "OrderBy", new Type[] { typeOfT, propertyType }, items.Expression, Expression.Quote(orderExpression));
                return items.Provider.CreateQuery<T>(expression);
            }
            catch
            {
                var typeOfT = typeof(T);
                propertyName = typeOfT.GetProperties()[0].Name;
                var parameter = Expression.Parameter(typeOfT, "parameter");
                var propertyType = typeOfT.GetProperty(propertyName).PropertyType;

                var propertyAccess = Expression.PropertyOrField(parameter, propertyName);
                var orderExpression = Expression.Lambda(propertyAccess, parameter);

                var expression = Expression.Call(typeof(Queryable), "OrderBy", new Type[] { typeOfT, propertyType }, items.Expression, Expression.Quote(orderExpression));
                return items.Provider.CreateQuery<T>(expression);
            }
        }

        public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> items, string propertyName)
        {
            try
            {
                var typeOfT = typeof(T);
                var parameter = Expression.Parameter(typeOfT, "parameter");
                var prop = typeOfT.GetProperties().Where(p => p.Name.ToLower() == propertyName.ToLower()).FirstOrDefault();
                propertyName = prop.Name;
                var propertyType = prop.PropertyType;
                var propertyAccess = Expression.PropertyOrField(parameter, propertyName);
                var orderExpression = Expression.Lambda(propertyAccess, parameter);

                var expression = Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { typeOfT, propertyType }, items.Expression, Expression.Quote(orderExpression));
                return items.Provider.CreateQuery<T>(expression);
            }
            catch
            {
                var typeOfT = typeof(T);
                propertyName = typeOfT.GetProperties()[0].Name;
                var parameter = Expression.Parameter(typeOfT, "parameter");
                var propertyType = typeOfT.GetProperty(propertyName).PropertyType;

                var propertyAccess = Expression.PropertyOrField(parameter, propertyName);
                var orderExpression = Expression.Lambda(propertyAccess, parameter);

                var expression = Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { typeOfT, propertyType }, items.Expression, Expression.Quote(orderExpression));
                return items.Provider.CreateQuery<T>(expression);
            }
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> query, Type typeOfT, string column, string value, WhereOperation operation)
        {
            if (string.IsNullOrEmpty(column) || string.IsNullOrEmpty(value))
                return query;

            ParameterExpression parameter = Expression.Parameter(typeOfT, "p");
            PropertyInfo prop = null;
            MemberExpression memberAccess = null;
            foreach (var property in column.Split('.'))
            {
                prop = typeOfT.GetProperties().Where(p => p.Name.ToLower() == property.ToLower()).FirstOrDefault();
                memberAccess = MemberExpression.Property(memberAccess ?? (parameter as Expression), prop);
            }
            //change param value type
            //necessary to getting bool from string
            object t = null;
            try { t = TypeDescriptor.GetConverter(memberAccess.Type).ConvertFromString(value); }
            catch
            {
                if (memberAccess.Type.IsValueType)
                    t = Activator.CreateInstance(memberAccess.Type);
            }
            ConstantExpression filter = Expression.Constant(t);
            //switch operation
            Expression condition = null;
            LambdaExpression lambda = null;
            switch (operation)
            {
                //equal ==
                case WhereOperation.Equal:
                    condition = Expression.Equal(memberAccess, Expression.Convert(filter, memberAccess.Type));
                    lambda = Expression.Lambda(condition, parameter);
                    break;
                //not equal !=
                case WhereOperation.NotEqual:
                    condition = Expression.NotEqual(memberAccess, Expression.Convert(filter, memberAccess.Type));
                    lambda = Expression.Lambda(condition, parameter);
                    break;
                //string.Contains()
                case WhereOperation.Contains:
                    condition = Expression.Call(memberAccess,
                        typeof(string).GetMethod("Contains"),
                        Expression.Constant(value));
                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case WhereOperation.Like:
                    Expression expression = null, nullCheck = null;
                    if (Nullable.GetUnderlyingType(memberAccess.Type) != null)
                    {
                        nullCheck = Expression.NotEqual(memberAccess, Expression.Constant(Activator.CreateInstance(memberAccess.Type), memberAccess.Type));
                        var memberAccesschild = MemberExpression.Property(memberAccess ?? (parameter as Expression), memberAccess.Type.GetProperty("Value"));
                        expression = Expression.Call(memberAccesschild, memberAccesschild.Type.GetMethod("ToString", System.Type.EmptyTypes));
                    }
                    else if (memberAccess.Type != typeof(string))
                    {
                        expression = Expression.Call(memberAccess, memberAccess.Type.GetMethod("ToString", System.Type.EmptyTypes));
                        //nullCheck = Expression.NotEqual(memberAccess, Expression.Constant(Activator.CreateInstance(memberAccess.Type), memberAccess.Type));
                    }
                    else
                    {
                        expression = memberAccess;
                        nullCheck = Expression.NotEqual(memberAccess, Expression.Constant(null, typeof(object)));
                    }
                    var toLower = Expression.Call(expression, typeof(string).GetMethod("ToLower", System.Type.EmptyTypes));
                    var compare = Expression.Call(toLower,
                        typeof(string).GetMethod("Contains"),
                        Expression.Constant(value.ToString().ToLower()));
                    if (nullCheck != null)
                    {
                        condition = Expression.AndAlso(nullCheck, compare);
                        lambda = Expression.Lambda(condition, parameter);
                    }
                    else
                    {
                        lambda = Expression.Lambda(compare, parameter);
                    }
                    break;
                case WhereOperation.Gretter:
                    condition = Expression.GreaterThan(memberAccess, Expression.Convert(filter, memberAccess.Type));
                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case WhereOperation.GretterEq:
                    condition = Expression.GreaterThanOrEqual(memberAccess, Expression.Convert(filter, memberAccess.Type));
                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case WhereOperation.Less:
                    condition = Expression.LessThan(memberAccess, Expression.Convert(filter, memberAccess.Type));
                    lambda = Expression.Lambda(condition, parameter);
                    break;
                case WhereOperation.LessEq:
                    condition = Expression.LessThanOrEqual(memberAccess, Expression.Convert(filter, memberAccess.Type));
                    lambda = Expression.Lambda(condition, parameter);
                    break;
            }
            Expression expressionObj = query.Expression;
            if (query.ElementType == typeof(object))
            {
                var queryObject1 = Expression.Call(typeof(Queryable), "Cast", new[] { typeOfT }, query.Expression);
                var queryObject = Expression.Call(typeof(Queryable), "AsQueryable", new[] { typeOfT }, queryObject1);
                MethodCallExpression result = Expression.Call(typeof(Queryable), "Where", new[] { typeOfT }, queryObject, lambda);
                return query.Provider.CreateQuery<T>(result);
            }
            else
            {
                MethodCallExpression result = Expression.Call(
                    typeof(Queryable), "Where",
                    new[] { typeOfT },
                    expressionObj,
                    lambda);
                return query.Provider.CreateQuery<T>(result);
            }
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> query, string column, string value, WhereOperation operation)
        {
            return query.Where(typeof(T), column, value, operation);
        }

        public static List<T> ApplyFilter<T>(this IQueryable<T> query, SearchModel model, out int total)
        {
            if (model.search.Count > 0)
                query = model.search.Aggregate(query, (current, item) => current.Where(item.Key, item.Value, WhereOperation.Like));
            if (!string.IsNullOrEmpty(model.sort))
                query = model.order == "desc" ? query.OrderByDescending(model.sort) : query.OrderBy(model.sort);
            else
                query = query.OrderByDescending("Id");
            total = query.Count();
            return query.Skip(model.offset).Take(model.limit).ToList();
        }

        public static List<ValidationResult> ValidateBeforeSave(this object entity)
        {
            var context = new ValidationContext(entity, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(entity, context, results);
            if (!isValid)
            {
                return results;
            }
            return new List<ValidationResult>();
        }

        public static dynamic[] ToPivotArray<T, TColumn, TRow, TData>(this IEnumerable<T> source, Func<T, TColumn> columnSelector, Expression<Func<T, TRow>> rowSelector, Func<IEnumerable<T>, TData> dataSelector)
        {

            var arr = new List<dynamic>();
            var cols = new List<string>();
            String rowName = ((MemberExpression)rowSelector.Body).Member.Name;
            var columns = source.Select(columnSelector).Distinct();

            cols = (new[] { rowName }).Concat(columns.Select(x => x.ToString())).ToList();


            var rows = source.GroupBy(rowSelector.Compile())
                             .Select(rowGroup => new
                             {
                                 Key = rowGroup.Key,
                                 Values = columns.GroupJoin(
                                     rowGroup,
                                     c => c,
                                     r => columnSelector(r),
                                     (c, columnGroup) => dataSelector(columnGroup))
                             }).ToArray();


            foreach (var row in rows)
            {
                var items = row.Values.Cast<object>().ToList();
                items.Insert(0, row.Key);
                var obj = GetAnonymousObject(cols, items);
                arr.Add(obj);
            }
            return arr.ToArray();
        }

        private static dynamic GetAnonymousObject(IEnumerable<string> columns, IEnumerable<object> values)
        {
            IDictionary<string, object> eo = new ExpandoObject() as IDictionary<string, object>;
            int i;
            for (i = 0; i < columns.Count(); i++)
            {
                eo.Add(columns.ElementAt<string>(i), values.ElementAt<object>(i));
            }
            return eo;
        }
    }
}