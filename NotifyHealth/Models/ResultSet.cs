using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace NotifyHealth.Models
{
    public class ResultSet
    {
        /// <summary>
        /// SPeed
        /// </summary>
        /// <param name="search"></param>
        /// <param name="dtResult"></param>
        /// <param name="columnFilters"></param>
        /// <returns></returns>
        public List<Programs> GetResult(string search, string sortOrder, int start, int length, List<Programs> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).SortBy(sortOrder).Skip(start).Take(length).ToList();
        }

        public int Count(string search, List<Programs> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).Count();
        }

        private IQueryable<Programs> FilterResult(string search, List<Programs> dtResult, List<string> columnFilters)
        {
            IQueryable<Programs> results = dtResult.AsQueryable();

            results = results.Where(p => (search == null || (p.Name.ToString().Contains(search.ToLower()) || p.Name != null && p.Name.ToString().Contains(search.ToLower()))));
            return results;
            /* && (columnFilters[3] == null || (p.InvoiceAmount.ToString() != null && p.InvoiceAmount.ToString().Contains(columnFilters[3].ToLower())))*/
            /*|| p.InvoiceAmount.ToString() != null && p.InvoiceAmount.ToStri5g().Contains(search.ToLower())*/
            //&& (columnFilters[2] == null || (p.Status != null && p.Status.ToString().Contains(columnFilters[2].ToLower()))

            //       && (columnFilters[3] == null || (p.Web != null && p.Web.ToLower().Contains(columnFilters[3].ToLower())))
            //&& (columnFilters[4] == null || (p.Company != null && p.Company.ToLower().Contains(columnFilters[4].ToLower())))
            //&& (columnFilters[5] == null || (p.QuoteType != null && p.QuoteType.ToLower().Contains(columnFilters[5].ToLower())))
            //&& (columnFilters[6] == null || (p.ValidUntil != null && p.ValidUntil.ToString().Contains(columnFilters[6].ToLower())))

            //    || p.Web != null && p.Web.ToLower().Contains(search.ToLower()) || p.Company != null && p.Company.ToLower().Contains(search.ToLower()) || p.QuoteType != null && p.QuoteType.ToLower().Contains(search.ToLower())
            //        || p.ValidUntil != null && p.ValidUntil.ToString().Contains(search.ToLower()
            //}
        }

        /// <summary>
        /// Campaigns
        /// </summary>
        /// <param name="search"></param>
        /// <param name="dtResult"></param>
        /// <param name="columnFilters"></param>
        /// <returns></returns>
        public List<Campaigns> GetResult(string search, string sortOrder, int start, int length, List<Campaigns> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).SortBy(sortOrder).Skip(start).Take(length).ToList();
        }

        public int Count(string search, List<Campaigns> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).Count();
        }

        private IQueryable<Campaigns> FilterResult(string search, List<Campaigns> dtResult, List<string> columnFilters)
        {
            IQueryable<Campaigns> results = dtResult.AsQueryable();

            results = results.Where(p => (search == null || (p.Description.ToString().Contains(search.ToLower()) || p.Description != null && p.Description.ToString().Contains(search.ToLower())))
                && (columnFilters[0] == null || (p.Name != null && p.Name.ToString().Contains(columnFilters[0].ToLower()))));
            return results;
            /* && (columnFilters[3] == null || (p.InvoiceAmount.ToString() != null && p.InvoiceAmount.ToString().Contains(columnFilters[3].ToLower())))*/
            /*|| p.InvoiceAmount.ToString() != null && p.InvoiceAmount.ToStri5g().Contains(search.ToLower())*/
            //&& (columnFilters[2] == null || (p.Status != null && p.Status.ToString().Contains(columnFilters[2].ToLower()))

            //       && (columnFilters[3] == null || (p.Web != null && p.Web.ToLower().Contains(columnFilters[3].ToLower())))
            //&& (columnFilters[4] == null || (p.Company != null && p.Company.ToLower().Contains(columnFilters[4].ToLower())))
            //&& (columnFilters[5] == null || (p.QuoteType != null && p.QuoteType.ToLower().Contains(columnFilters[5].ToLower())))
            //&& (columnFilters[6] == null || (p.ValidUntil != null && p.ValidUntil.ToString().Contains(columnFilters[6].ToLower())))

            //    || p.Web != null && p.Web.ToLower().Contains(search.ToLower()) || p.Company != null && p.Company.ToLower().Contains(search.ToLower()) || p.QuoteType != null && p.QuoteType.ToLower().Contains(search.ToLower())
            //        || p.ValidUntil != null && p.ValidUntil.ToString().Contains(search.ToLower()
            //}
        }

        /// <summary>
        /// Notifications
        /// </summary>
        /// <param name="search"></param>
        /// <param name="dtResult"></param>
        /// <param name="columnFilters"></param>
        /// <returns></returns>
        public List<Notifications> GetResult(string search, string sortOrder, int start, int length, List<Notifications> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).SortBy(sortOrder).Skip(start).Take(length).ToList();
        }

        public int Count(string search, List<Notifications> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).Count();
        }

        private IQueryable<Notifications> FilterResult(string search, List<Notifications> dtResult, List<string> columnFilters)
        {
            IQueryable<Notifications> results = dtResult.AsQueryable();

            results = results.Where(p => (search == null || (p.Period.ToString().Contains(search.ToLower()) || p.Period.ToString() != null && p.Period.ToString().Contains(search.ToLower())))
                && (columnFilters[0] == null || (p.Text != null && p.Text.ToString().Contains(columnFilters[0].ToLower()))));
            return results;
            /* && (columnFilters[3] == null || (p.InvoiceAmount.ToString() != null && p.InvoiceAmount.ToString().Contains(columnFilters[3].ToLower())))*/
            /*|| p.InvoiceAmount.ToString() != null && p.InvoiceAmount.ToStri5g().Contains(search.ToLower())*/
            //&& (columnFilters[2] == null || (p.Status != null && p.Status.ToString().Contains(columnFilters[2].ToLower()))

            //       && (columnFilters[3] == null || (p.Web != null && p.Web.ToLower().Contains(columnFilters[3].ToLower())))
            //&& (columnFilters[4] == null || (p.Company != null && p.Company.ToLower().Contains(columnFilters[4].ToLower())))
            //&& (columnFilters[5] == null || (p.QuoteType != null && p.QuoteType.ToLower().Contains(columnFilters[5].ToLower())))
            //&& (columnFilters[6] == null || (p.ValidUntil != null && p.ValidUntil.ToString().Contains(columnFilters[6].ToLower())))

            //    || p.Web != null && p.Web.ToLower().Contains(search.ToLower()) || p.Company != null && p.Company.ToLower().Contains(search.ToLower()) || p.QuoteType != null && p.QuoteType.ToLower().Contains(search.ToLower())
            //        || p.ValidUntil != null && p.ValidUntil.ToString().Contains(search.ToLower()
            //}
        }

        /// <summary>
        /// Clients
        /// </summary>
        /// <param name="search"></param>
        /// <param name="dtResult"></param>
        /// <param name="columnFilters"></param>
        /// <returns></returns>
        public List<Clients> GetResult(string search, string sortOrder, int start, int length, List<Clients> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).SortBy(sortOrder).Skip(start).Take(length).ToList();
        }

        public int Count(string search, List<Clients> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).Count();
        }

        private IQueryable<Clients> FilterResult(string search, List<Clients> dtResult, List<string> columnFilters)
        {
            IQueryable<Clients> results = dtResult.AsQueryable();

            results = results.Where(p => (search == null || (p.FirstName.ToString().Contains(search.ToLower()) || p.FirstName != null && p.FirstName.ToString().Contains(search.ToLower())))
                && (columnFilters[0] == null || (p.FirstName != null && p.FirstName.ToString().Contains(columnFilters[0].ToLower()))));
            return results;
            /* && (columnFilters[3] == null || (p.InvoiceAmount.ToString() != null && p.InvoiceAmount.ToString().Contains(columnFilters[3].ToLower())))*/
            /*|| p.InvoiceAmount.ToString() != null && p.InvoiceAmount.ToStri5g().Contains(search.ToLower())*/
            //&& (columnFilters[2] == null || (p.Status != null && p.Status.ToString().Contains(columnFilters[2].ToLower()))

            //       && (columnFilters[3] == null || (p.Web != null && p.Web.ToLower().Contains(columnFilters[3].ToLower())))
            //&& (columnFilters[4] == null || (p.Company != null && p.Company.ToLower().Contains(columnFilters[4].ToLower())))
            //&& (columnFilters[5] == null || (p.QuoteType != null && p.QuoteType.ToLower().Contains(columnFilters[5].ToLower())))
            //&& (columnFilters[6] == null || (p.ValidUntil != null && p.ValidUntil.ToString().Contains(columnFilters[6].ToLower())))

            //    || p.Web != null && p.Web.ToLower().Contains(search.ToLower()) || p.Company != null && p.Company.ToLower().Contains(search.ToLower()) || p.QuoteType != null && p.QuoteType.ToLower().Contains(search.ToLower())
            //        || p.ValidUntil != null && p.ValidUntil.ToString().Contains(search.ToLower()
            //}
        }

        /// <summary>
        /// Clients
        /// </summary>
        /// <param name="search"></param>
        /// <param name="dtResult"></param>
        /// <param name="columnFilters"></param>
        /// <returns></returns>
        public List<ClientMemberships> GetResult(string search, string sortOrder, int start, int length, List<ClientMemberships> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).SortBy(sortOrder).Skip(start).Take(length).ToList();
        }

        public int Count(string search, List<ClientMemberships> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).Count();
        }

        private IQueryable<ClientMemberships> FilterResult(string search, List<ClientMemberships> dtResult, List<string> columnFilters)
        {
            IQueryable<ClientMemberships> results = dtResult.AsQueryable();

            results = results.Where(p => (search == null || (p.Start.ToString().Contains(search.ToLower()) || p.Start != null && p.Start.ToString().Contains(search.ToLower())))
                && (columnFilters[0] == null || (p.Campaign != null && p.Campaign.ToString().Contains(columnFilters[0].ToLower()))));
            return results;
            /* && (columnFilters[3] == null || (p.InvoiceAmount.ToString() != null && p.InvoiceAmount.ToString().Contains(columnFilters[3].ToLower())))*/
            /*|| p.InvoiceAmount.ToString() != null && p.InvoiceAmount.ToStri5g().Contains(search.ToLower())*/
            //&& (columnFilters[2] == null || (p.Status != null && p.Status.ToString().Contains(columnFilters[2].ToLower()))

            //       && (columnFilters[3] == null || (p.Web != null && p.Web.ToLower().Contains(columnFilters[3].ToLower())))
            //&& (columnFilters[4] == null || (p.Company != null && p.Company.ToLower().Contains(columnFilters[4].ToLower())))
            //&& (columnFilters[5] == null || (p.QuoteType != null && p.QuoteType.ToLower().Contains(columnFilters[5].ToLower())))
            //&& (columnFilters[6] == null || (p.ValidUntil != null && p.ValidUntil.ToString().Contains(columnFilters[6].ToLower())))

            //    || p.Web != null && p.Web.ToLower().Contains(search.ToLower()) || p.Company != null && p.Company.ToLower().Contains(search.ToLower()) || p.QuoteType != null && p.QuoteType.ToLower().Contains(search.ToLower())
            //        || p.ValidUntil != null && p.ValidUntil.ToString().Contains(search.ToLower()
            //}
        }

        /// <summary>
        /// Clients
        /// </summary>
        /// <param name="search"></param>
        /// <param name="dtResult"></param>
        /// <param name="columnFilters"></param>
        /// <returns></returns>
        public List<Transactions> GetResult(string search, string sortOrder, int start, int length, List<Transactions> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).SortBy(sortOrder).Skip(start).Take(length).ToList();
        }

        public int Count(string search, List<Transactions> dtResult, List<string> columnFilters)
        {
            return FilterResult(search, dtResult, columnFilters).Count();
        }

        private IQueryable<Transactions> FilterResult(string search, List<Transactions> dtResult, List<string> columnFilters)
        {
            IQueryable<Transactions> results = dtResult.AsQueryable();

            results = results.Where(p => (search == null || (p.Timestamp.ToString().Contains(search.ToLower()) || p.Timestamp != null && p.Timestamp.ToString().Contains(search.ToLower())))
                && (columnFilters[0] == null || (p.Notification != null && p.Notification.ToString().Contains(columnFilters[0].ToLower()))));
            return results;
        }
    }
}