using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Web;
using DevExpress.Internal;

namespace DataContext {

    public partial class SalesDataContext {
        private const int BatchSize = 2500;

        protected DataGenerator fDataGenerator = null;
        protected DataGenerator DataGenerator {
            get {
                if(fDataGenerator == null)
                    fDataGenerator = new DataGenerator();
                return fDataGenerator;
            }
        }

        protected static string ConnectionString {
            get { return DbEngineDetector.PatchConnectionString(ConfigurationManager.ConnectionStrings["SalesViewerConnectionString"].ConnectionString); }
        }
        public SalesDataContext()
            : base(ConnectionString) {
        }

        public static bool IsDatabasePopulated() {
            using(SalesDataContext dataContext = new SalesDataContext())
                return dataContext.IsDatabaseTablesPopulated();
        }
        public static void PopulateDatabaseIfNecessary() {
            if(!IsDatabasePopulated()) {
                using(SalesDataContext dataContext = new SalesDataContext())
                    dataContext.PopulateDatabase();
            }
        }
        public static int DatabasePopulatingProgressPercentValue { get; private set; }

        private bool IsDatabaseTablesPopulated() {
            return Regions.Any() && Cities.Any() && Channels.Any() && Sectors.Any() && Contacts.Any() &&
                    Customers.Any() && Plants.Any() && Products.Any() && Sales.Any();
        }

        private void PopulateDatabase() {
            PopulateTable<Region>(DataGenerator.Regions);
            PopulateTable<City>(DataGenerator.Cities);
            PopulateTable<Channel>(DataGenerator.Channels);
            PopulateTable<Sector>(DataGenerator.Sectors);
            PopulateTable<Contact>(DataGenerator.Contacts);
            PopulateTable<Customer>(DataGenerator.Customers);
            PopulateTable<Plant>(DataGenerator.Plants);
            PopulateTable<Product>(DataGenerator.Products);
            PopulateTable<Sale>(DataGenerator.Sales);
        }
        private void PopulateTable<T>(IEnumerable<T> data) where T : class {
            Table<T> dataTable = GetTable<T>();
            int tableRowCount = dataTable.Count();
            if(tableRowCount < data.Count())
                BulkInsert<T>(data.Skip(tableRowCount), dataTable);
        }
        private void BulkInsert<T>(IEnumerable<T> data, Table<T> table) where T : class {
            int maxPage = (int)Math.Ceiling(data.Count() / (double)BatchSize);
            for(int page = 0; page < maxPage; page++) {
                table.InsertAllOnSubmit(data.Skip(page * BatchSize).Take(BatchSize));
                SubmitChanges();
                // Progress
                DatabasePopulatingProgressPercentValue = (int)(page * 100 / maxPage);
            }
        }
    }
}
