using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using productconsole.Models;

namespace productconsole.Data
{
    public class DataService
    {
        public string FilePath { get; set; }
        public DataService(string filePath)
        {
            FilePath = filePath;
        }
        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            try
            {
                using (SpreadsheetDocument sd = SpreadsheetDocument.Open(FilePath, false))
                {
                    Sheet sheet = sd.WorkbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name.Equals("Товары"));

                    Worksheet worksheet = (sd.WorkbookPart.GetPartById(sheet.Id.Value) as WorksheetPart).Worksheet;

                    IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();

                    int counter = 0;

                    foreach (Row row in rows)
                    {
                        counter++;

                        if (counter == 1)
                        {
                            continue;
                        }
                        else
                        {
                            var product = GetProduct(sd, row);
                            if (product != null)
                                products.Add(product);
                            else
                                continue;
                        }
                    }
                }
            }
            catch (Exception ex) { }

            return products.OrderBy(x => x.Id).ToList();
        }
        public List<Client> GetClients()
        {
            List<Client> clients = new List<Client>();
            try
            {
                using (SpreadsheetDocument sd = SpreadsheetDocument.Open(FilePath, false))
                {
                    Sheet sheet = sd.WorkbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name.Equals("Клиенты"));

                    Worksheet worksheet = (sd.WorkbookPart.GetPartById(sheet.Id.Value) as WorksheetPart).Worksheet;

                    IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();

                    int counter = 0;

                    foreach (Row row in rows)
                    {
                        counter++;
                        if (counter == 1)
                        {
                            continue;
                        }
                        else
                        {
                            var client = GetClient(sd, row);
                            if (client != null)
                                clients.Add(client);
                            else
                                continue;
                        }
                    }
                }
            }
            catch(Exception ex) { }
            return clients.OrderBy(x => x.Id).ToList();
        }
        public List<Order> GetOrders()
        {
            List<Order> orders = new List<Order>();

            try
            {
                using (SpreadsheetDocument sd = SpreadsheetDocument.Open(FilePath, false))
                {
                    Sheet sheet = sd.WorkbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name.Equals("Заявки"));

                    Worksheet worksheet = (sd.WorkbookPart.GetPartById(sheet.Id.Value) as WorksheetPart).Worksheet;

                    IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();

                    int counter = 0;

                    foreach (Row row in rows)
                    {
                        counter++;

                        if (counter == 1)
                        {
                            continue;
                        }
                        else
                        {
                            var order = GetOrder(sd, row);
                            if (order != null)
                                orders.Add(order);
                            else
                                continue;
                        }
                    }
                }
            }
            catch (Exception ex) { }
            return orders.OrderBy(x => x.Id).ToList();
        }

        public bool UpdateClientContactPerson(int clientId, string newContact)
        {
            try
            {
                using (SpreadsheetDocument sd = SpreadsheetDocument.Open(FilePath, true))
                {
                    Sheet sheet = sd.WorkbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.Name.Equals("Клиенты"));

                    Worksheet worksheet = (sd.WorkbookPart.GetPartById(sheet.Id.Value) as WorksheetPart).Worksheet;

                    IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();

                    int counter = 0;

                    foreach (Row row in rows)
                    {
                        counter++;
                        if (counter == 1)
                        {
                            continue;
                        }
                        else
                        {
                            string searchedCellAdress = SearchAddressOfContactPerson(clientId, sd, row);

                            if (searchedCellAdress != null)
                            {
                                return UpdateContactPerson(searchedCellAdress, worksheet, sd, newContact);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }

            return false;
        }
        private string GetCellValue(SpreadsheetDocument doc, Cell cell)
        {
            string? value = null;
            try
            {

                value = cell.CellValue == null ? null : cell.CellValue.InnerText;
                if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                {
                    return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText;
                }
            }
            catch (Exception ex)
            {

            }
            return value;
        }
        private Product GetProduct(SpreadsheetDocument doc, Row row)
        {
            List<string> rowValues = GetRowValues(doc,row);

            Product? product = null;

            if (rowValues.Count == 4)
            {
                try
                {
                    product = new Product()
                    {
                        Id = Int32.Parse(rowValues[0]),
                        Name = rowValues[1],
                        Unit = rowValues[2],
                        Price = Decimal.Parse(rowValues[3])
                    };
                }
                catch (Exception ex) { }
            }

            return product;

        }
        private Client GetClient(SpreadsheetDocument doc, Row row) 
        {
            List<string>? rowValues = GetRowValues(doc, row);

            Client? client = null;

            if(rowValues.Count == 4)
            {
                try
                {
                    client = new Client()
                    {
                        Id = Int32.Parse(rowValues[0]),
                        Name = rowValues[1],
                        Address = rowValues[2],
                        ContactPerson = rowValues[3]
                    };
                }
                catch (Exception ex) { }
            }

            return client;
        }
        private Order GetOrder(SpreadsheetDocument doc, Row row)
        {
            List<string>? rowValues = GetRowValues(doc, row); ;

            Order? order = null;

            if (rowValues.Count == 6) {
                try {
                    order = new Order()
                    {
                        Id = Int32.Parse(rowValues[0]),
                        ProductId = Int32.Parse(rowValues[1]),
                        ClientId = Int32.Parse(rowValues[2]),
                        OrderNumber = Int32.Parse(rowValues[3]),
                        ProductCount = Int32.Parse(rowValues[4]),
                        OrderDate = DateTime.FromOADate(Double.Parse(rowValues[5]))
                    };
                } catch (Exception ex) { }
            }
            return order;
        }
        private List<string> GetRowValues(SpreadsheetDocument doc, Row row)
        {
            List<string>? rowValues = new List<string>();

            foreach (Cell cell in row.Descendants<Cell>())
            {
                string value = GetCellValue(doc, cell);

                if (value != null) 
                    rowValues.Add(value); 
                else
                    return new List<string>();

            }

            return rowValues;
        }
        private string SearchAddressOfContactPerson(int clientId, SpreadsheetDocument doc, Row row)
        {
            string? cellValue = null;
            try
            {
                Cell? cell = row.Descendants<Cell>().FirstOrDefault();

                var index = Int32.Parse(GetCellValue(doc, cell));

                if (index == clientId)
                {

                    cellValue = cell.CellReference.Value;
                }
            }
            catch(Exception ex) { }

            return cellValue == null? null : cellValue.Replace('A', 'D');
        }

        private bool UpdateContactPerson(string address, Worksheet worksheet, SpreadsheetDocument sd, string  contactPerson)
        {
            try
            {
                var theCell = worksheet.Descendants<Cell>().Where(c => c.CellReference == address).FirstOrDefault();

                if (theCell.DataType != null && theCell.DataType == CellValues.SharedString)
                {
                    var sharedStringTable = sd.WorkbookPart.SharedStringTablePart.SharedStringTable;
                    var element = sharedStringTable.ChildElements[int.Parse(theCell.InnerText)];
                    if (string.IsNullOrEmpty(element.InnerText))
                    {
                        SharedStringItem sst = new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(contactPerson));
                        sharedStringTable.AppendChild<SharedStringItem>(sst);
                        int value = sharedStringTable.ChildElements.ToList().IndexOf(sst);
                        theCell.CellValue = new CellValue(value.ToString());
                        sharedStringTable.Save();
                    }
                    else
                    {
                        element.InnerXml = element.InnerXml.Replace(element.InnerText, contactPerson);
                        sharedStringTable.Save();
                    }
                }
                else
                {
                    theCell.CellValue = new CellValue(contactPerson);
                    theCell.DataType = new EnumValue<CellValues>(CellValues.String);
                    worksheet.Save();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
