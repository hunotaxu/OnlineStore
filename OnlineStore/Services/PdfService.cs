using DAL.Data.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Commons;

namespace OnlineStore.Services
{
    public class PdfService : IPdfService
    {

        private readonly IPdfSettings _pdfSettings;
        private readonly IHostingEnvironment _hostingEnvironment;
        //private readonly ISettings _pdfSettings;
        //private readonly INopFileProvider _fileProvider;
        //private readonly ISettingService _settingService;
        //private readonly ILocalizationService _localizationService;

        //public PdfService(IPdfSettings pdfSettings, INopFileProvider fileProvider, IHostingEnvironment hostingEnvironment)
        public PdfService(IPdfSettings pdfSettings, IHostingEnvironment hostingEnvironment)
        //public PdfService(ISettings pdfSettings, INopFileProvider fileProvider)
        {
            _pdfSettings = pdfSettings;
            _hostingEnvironment = hostingEnvironment;
            //_fileProvider = fileProvider;
        }

        //protected virtual Font GetFont()
        //{
        //    //nopCommerce supports Unicode characters
        //    //nopCommerce uses Free Serif font by default (~/App_Data/Pdf/FreeSerif.ttf file)
        //    //It was downloaded from http://savannah.gnu.org/projects/freefont
        //    return GetFont(_pdfSettings.FontFileName);
        //}

        /// <summary>
        /// Get font
        /// </summary>
        /// <param name="fontFileName">Font file name</param>
        /// <returns>Font</returns>
        //protected virtual Font GetFont(string fontFileName)
        protected virtual Font GetFont()
        {
            //if (fontFileName == null)
            //    throw new ArgumentNullException(nameof(fontFileName));
            var fontFolder = $@"\fonts\font-for-print\";
            //var fontPath = _fileProvider.Combine(_fileProvider.MapPath("~/App_Data/Pdf/"), fontFileName);
            var fontPath = Path.Combine(_hostingEnvironment.WebRootPath + fontFolder, "FreeSerif.ttf");
            var baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            var font = new Font(baseFont, 10, Font.NORMAL);
            return font;
        }

        public virtual void PrintOrdersToPdf(Stream stream, Order order, int languageId = 0, int vendorId = 0)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var pageSize = PageSize.A4;

            if (_pdfSettings.LetterPageSizeEnabled)
            {
                pageSize = PageSize.LETTER;
            }

            var doc = new Document(pageSize);
            var pdfWriter = PdfWriter.GetInstance(doc, stream);
            doc.Open();

            //fonts
            var titleFont = GetFont();
            titleFont.SetStyle(Font.BOLD);
            titleFont.Color = BaseColor.BLACK;
            var font = GetFont();
            var attributesFont = GetFont();
            attributesFont.SetStyle(Font.ITALIC);

            //var ordCount = orders.Count;
            //var ordNum = 0;

            //by default _pdfSettings contains settings for the current active store
            //and we need PdfSettings for the store which was used to place an order
            //so let's load it based on a store of the current order
            //var pdfSettingsByStore = _settingService.LoadSetting<PdfSettings>(order.StoreId);
            //var pdfSettingsByStore = _settingService.LoadSetting<PdfSettings>(0);

            //var lang = _languageService.GetLanguageById(languageId == 0 ? order.CustomerLanguageId : languageId);
            //if (lang == null || !lang.Published)
            //    lang = _workContext.WorkingLanguage; PrintBillingInfo

            //header
            //PrintHeader(pdfSettingsByStore, lang, order, font, titleFont, doc);
            //PrintBillingInfo(titleFont, order, font, doc);
            PrintHeader(order, font, titleFont, doc);
            PrintBillingInfo(titleFont, order, font, doc);

            //addresses
            //PrintAddresses(vendorId, lang, titleFont, order, font, doc);
            //PrintAddresses(titleFont, order, font, doc);

            //products
            //PrintProducts(vendorId, lang, titleFont, doc, order, font, attributesFont);
            PrintProducts(titleFont, doc, order, font, attributesFont);

            //checkout attributes
            //PrintCheckoutAttributes(vendorId, order, doc, lang, font);
            //PrintCheckoutAttributes(vendorId, order, doc, font);

            //totals
            //PrintTotals(vendorId, lang, order, font, titleFont, doc);
            PrintTotals(order, font, titleFont, doc);

            //order notes
            //PrintOrderNotes(pdfSettingsByStore, order, lang, titleFont, doc, font);
            //PrintOrderNotes(order, titleFont, doc, font);

            //footer
            //PrintFooter(pdfSettingsByStore, pdfWriter, pageSize, lang, font);
            //PrintFooter(pdfWriter, pageSize, font);

            //ordNum++;
            //if (ordNum < ordCount)
            //{
            //    doc.NewPage();
            //}

            doc.Close();
        }

        protected virtual void PrintTotals(Order order, Font font, Font titleFont, Document doc)
        {
            //subtotal
            var totalsTable = new PdfPTable(1)
            {
                //RunDirection = GetDirection(lang),
                WidthPercentage = 100f
            };
            totalsTable.DefaultCell.Border = Rectangle.NO_BORDER;

            //order subtotal
            //if (order.CustomerTaxDisplayType == TaxDisplayType.IncludingTax &&
            //    !_taxSettings.ForceTaxExclusionFromOrderSubtotal)
            //{
            //    //including tax

            //    var orderSubtotalInclTaxInCustomerCurrency =
            //        _currencyService.ConvertCurrency(order.OrderSubtotalInclTax, order.CurrencyRate);
            //    var orderSubtotalInclTaxStr = _priceFormatter.FormatPrice(orderSubtotalInclTaxInCustomerCurrency, true,
            //        order.CustomerCurrencyCode, lang, true);

            //    var p = GetPdfCell($"{_localizationService.GetResource("PDFInvoice.Sub-Total", lang.Id)} {orderSubtotalInclTaxStr}", font);
            //    p.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    p.Border = Rectangle.NO_BORDER;
            //    totalsTable.AddCell(p);
            //}
            //else
            //{
            //    //excluding tax

            //    var orderSubtotalExclTaxInCustomerCurrency =
            //        _currencyService.ConvertCurrency(order.OrderSubtotalExclTax, order.CurrencyRate);
            //    var orderSubtotalExclTaxStr = _priceFormatter.FormatPrice(orderSubtotalExclTaxInCustomerCurrency, true,
            //        order.CustomerCurrencyCode, lang, false);

            //    var p = GetPdfCell($"{_localizationService.GetResource("PDFInvoice.Sub-Total", lang.Id)} {orderSubtotalExclTaxStr}", font);
            //    p.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    p.Border = Rectangle.NO_BORDER;
            //    totalsTable.AddCell(p);
            //}

            var orderSubtotalStr = CommonFunctions.FormatPrice(order.SubTotal);

            var p = GetPdfCell($"Tạm tính: {orderSubtotalStr}", font);
            p.HorizontalAlignment = Element.ALIGN_RIGHT;
            p.Border = Rectangle.NO_BORDER;
            totalsTable.AddCell(p);

            //discount (applied to order subtotal)
            //if (order.OrderSubTotalDiscountExclTax > decimal.Zero)
            if (order.SaleOff > decimal.Zero)
            {
                //order subtotal
                //if (order.CustomerTaxDisplayType == TaxDisplayType.IncludingTax &&
                //    !_taxSettings.ForceTaxExclusionFromOrderSubtotal)
                //{
                //    //including tax

                //    var orderSubTotalDiscountInclTaxInCustomerCurrency =
                //        _currencyService.ConvertCurrency(order.OrderSubTotalDiscountInclTax, order.CurrencyRate);
                //    var orderSubTotalDiscountInCustomerCurrencyStr = _priceFormatter.FormatPrice(
                //        -orderSubTotalDiscountInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, lang, true);

                //    var p = GetPdfCell($"{_localizationService.GetResource("PDFInvoice.Discount", lang.Id)} {orderSubTotalDiscountInCustomerCurrencyStr}", font);
                //    p.HorizontalAlignment = Element.ALIGN_RIGHT;
                //    p.Border = Rectangle.NO_BORDER;
                //    totalsTable.AddCell(p);
                //}
                //else
                //{
                //    //excluding tax

                //    var orderSubTotalDiscountExclTaxInCustomerCurrency =
                //        _currencyService.ConvertCurrency(order.OrderSubTotalDiscountExclTax, order.CurrencyRate);
                //    var orderSubTotalDiscountInCustomerCurrencyStr = _priceFormatter.FormatPrice(
                //        -orderSubTotalDiscountExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, lang, false);

                //    var p = GetPdfCell($"{_localizationService.GetResource("PDFInvoice.Discount", lang.Id)} {orderSubTotalDiscountInCustomerCurrencyStr}", font);
                //    p.HorizontalAlignment = Element.ALIGN_RIGHT;
                //    p.Border = Rectangle.NO_BORDER;
                //    totalsTable.AddCell(p);
                //}
            }

            //shipping
            //if (order.ShippingStatus != ShippingStatus.ShippingNotRequired)
            //{
            //    if (order.CustomerTaxDisplayType == TaxDisplayType.IncludingTax)
            //    {
            //        //including tax
            //        var orderShippingInclTaxInCustomerCurrency =
            //            _currencyService.ConvertCurrency(order.OrderShippingInclTax, order.CurrencyRate);
            //        var orderShippingInclTaxStr = _priceFormatter.FormatShippingPrice(
            //            orderShippingInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, lang, true);

            //        var p = GetPdfCell($"{_localizationService.GetResource("PDFInvoice.Shipping", lang.Id)} {orderShippingInclTaxStr}", font);
            //        p.HorizontalAlignment = Element.ALIGN_RIGHT;
            //        p.Border = Rectangle.NO_BORDER;
            //        totalsTable.AddCell(p);
            //    }
            //    else
            //    {
            //        //excluding tax
            //        var orderShippingExclTaxInCustomerCurrency =
            //            _currencyService.ConvertCurrency(order.OrderShippingExclTax, order.CurrencyRate);
            //        var orderShippingExclTaxStr = _priceFormatter.FormatShippingPrice(
            //            orderShippingExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, lang, false);

            //        var p = GetPdfCell($"{_localizationService.GetResource("PDFInvoice.Shipping", lang.Id)} {orderShippingExclTaxStr}", font);
            //        p.HorizontalAlignment = Element.ALIGN_RIGHT;
            //        p.Border = Rectangle.NO_BORDER;
            //        totalsTable.AddCell(p);
            //    }
            //}

            if (order.ShippingFee.HasValue)
            {
                var orderShippingStr = CommonFunctions.FormatPrice(order.ShippingFee.Value);
                var pdfShipping = GetPdfCell($"Phí vận chuyển: {orderShippingStr}", font);
                pdfShipping.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdfShipping.Border = Rectangle.NO_BORDER;
                totalsTable.AddCell(pdfShipping);
            }

            //payment fee
            //if (order.PaymentMethodAdditionalFeeExclTax > decimal.Zero)
            //{
            //    if (order.CustomerTaxDisplayType == TaxDisplayType.IncludingTax)
            //    {
            //        //including tax
            //        var paymentMethodAdditionalFeeInclTaxInCustomerCurrency =
            //            _currencyService.ConvertCurrency(order.PaymentMethodAdditionalFeeInclTax, order.CurrencyRate);
            //        var paymentMethodAdditionalFeeInclTaxStr = _priceFormatter.FormatPaymentMethodAdditionalFee(
            //            paymentMethodAdditionalFeeInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, lang, true);

            //        var p = GetPdfCell($"{_localizationService.GetResource("PDFInvoice.PaymentMethodAdditionalFee", lang.Id)} {paymentMethodAdditionalFeeInclTaxStr}", font);
            //        p.HorizontalAlignment = Element.ALIGN_RIGHT;
            //        p.Border = Rectangle.NO_BORDER;
            //        totalsTable.AddCell(p);
            //    }
            //    else
            //    {
            //        //excluding tax
            //        var paymentMethodAdditionalFeeExclTaxInCustomerCurrency =
            //            _currencyService.ConvertCurrency(order.PaymentMethodAdditionalFeeExclTax, order.CurrencyRate);
            //        var paymentMethodAdditionalFeeExclTaxStr = _priceFormatter.FormatPaymentMethodAdditionalFee(
            //            paymentMethodAdditionalFeeExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, lang, false);

            //        var p = GetPdfCell($"{_localizationService.GetResource("PDFInvoice.PaymentMethodAdditionalFee", lang.Id)} {paymentMethodAdditionalFeeExclTaxStr}", font);
            //        p.HorizontalAlignment = Element.ALIGN_RIGHT;
            //        p.Border = Rectangle.NO_BORDER;
            //        totalsTable.AddCell(p);
            //    }
            //}

            ////tax
            //var taxStr = string.Empty;
            //var taxRates = new SortedDictionary<decimal, decimal>();
            //bool displayTax;
            //var displayTaxRates = true;
            //if (_taxSettings.HideTaxInOrderSummary && order.CustomerTaxDisplayType == TaxDisplayType.IncludingTax)
            //{
            //    displayTax = false;
            //}
            //else
            //{
            //    if (order.OrderTax == 0 && _taxSettings.HideZeroTax)
            //    {
            //        displayTax = false;
            //        displayTaxRates = false;
            //    }
            //    else
            //    {
            //        taxRates = _orderService.ParseTaxRates(order, order.TaxRates);

            //        displayTaxRates = _taxSettings.DisplayTaxRates && taxRates.Any();
            //        displayTax = !displayTaxRates;

            //        var orderTaxInCustomerCurrency = _currencyService.ConvertCurrency(order.OrderTax, order.CurrencyRate);
            //        taxStr = _priceFormatter.FormatPrice(orderTaxInCustomerCurrency, true, order.CustomerCurrencyCode,
            //            false, lang);
            //    }
            //}

            //if (displayTax)
            //{
            //    var p = GetPdfCell($"{_localizationService.GetResource("PDFInvoice.Tax", lang.Id)} {taxStr}", font);
            //    p.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    p.Border = Rectangle.NO_BORDER;
            //    totalsTable.AddCell(p);
            //}

            //if (displayTaxRates)
            //{
            //    foreach (var item in taxRates)
            //    {
            //        var taxRate = string.Format(_localizationService.GetResource("PDFInvoice.TaxRate", lang.Id),
            //            _priceFormatter.FormatTaxRate(item.Key));
            //        var taxValue = _priceFormatter.FormatPrice(
            //            _currencyService.ConvertCurrency(item.Value, order.CurrencyRate), true, order.CustomerCurrencyCode,
            //            false, lang);

            //        var p = GetPdfCell($"{taxRate} {taxValue}", font);
            //        p.HorizontalAlignment = Element.ALIGN_RIGHT;
            //        p.Border = Rectangle.NO_BORDER;
            //        totalsTable.AddCell(p);
            //    }
            //}

            //discount (applied to order total)
            //if (order.OrderDiscount > decimal.Zero)
            //{
            //    var orderDiscountInCustomerCurrency =
            //        _currencyService.ConvertCurrency(order.OrderDiscount, order.CurrencyRate);
            //    var orderDiscountInCustomerCurrencyStr = _priceFormatter.FormatPrice(-orderDiscountInCustomerCurrency,
            //        true, order.CustomerCurrencyCode, false, lang);

            //    var p = GetPdfCell($"{_localizationService.GetResource("PDFInvoice.Discount", lang.Id)} {orderDiscountInCustomerCurrencyStr}", font);
            //    p.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    p.Border = Rectangle.NO_BORDER;
            //    totalsTable.AddCell(p);
            //}

            if (order.SaleOff.HasValue && order.SaleOff > decimal.Zero)
            {
                var orderSubTotalDiscountStr = CommonFunctions.FormatPrice(order.SaleOff.Value);

                var pdfSaleOff = GetPdfCell($"Giảm giá: {orderSubTotalDiscountStr}", font);
                pdfSaleOff.HorizontalAlignment = Element.ALIGN_RIGHT;
                pdfSaleOff.Border = Rectangle.NO_BORDER;
                totalsTable.AddCell(pdfSaleOff);
            }

            ////gift cards
            //foreach (var gcuh in order.GiftCardUsageHistory)
            //{
            //    var gcTitle = string.Format(_localizationService.GetResource("PDFInvoice.GiftCardInfo", lang.Id),
            //        gcuh.GiftCard.GiftCardCouponCode);
            //    var gcAmountStr = _priceFormatter.FormatPrice(
            //        -_currencyService.ConvertCurrency(gcuh.UsedValue, order.CurrencyRate), true,
            //        order.CustomerCurrencyCode, false, lang);

            //    var p = GetPdfCell($"{gcTitle} {gcAmountStr}", font);
            //    p.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    p.Border = Rectangle.NO_BORDER;
            //    totalsTable.AddCell(p);
            //}

            ////reward points
            //if (order.RedeemedRewardPointsEntry != null)
            //{
            //    var rpTitle = string.Format(_localizationService.GetResource("PDFInvoice.RewardPoints", lang.Id),
            //        -order.RedeemedRewardPointsEntry.Points);
            //    var rpAmount = _priceFormatter.FormatPrice(
            //        -_currencyService.ConvertCurrency(order.RedeemedRewardPointsEntry.UsedAmount, order.CurrencyRate),
            //        true, order.CustomerCurrencyCode, false, lang);

            //    var p = GetPdfCell($"{rpTitle} {rpAmount}", font);
            //    p.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    p.Border = Rectangle.NO_BORDER;
            //    totalsTable.AddCell(p);
            //}

            //order total
            //var orderTotalInCustomerCurrency = _currencyService.ConvertCurrency(order.OrderTotal, order.CurrencyRate);
            //var orderTotalStr = _priceFormatter.FormatPrice(orderTotalInCustomerCurrency, true, order.CustomerCurrencyCode, false, lang);

            var orderTotalStr = CommonFunctions.FormatPrice(order.Total.HasValue ? order.Total.Value : 0);

            var pTotal = GetPdfCell($"Tổng cộng: {orderTotalStr}", titleFont);
            pTotal.HorizontalAlignment = Element.ALIGN_RIGHT;
            pTotal.Border = Rectangle.NO_BORDER;
            totalsTable.AddCell(pTotal);

            doc.Add(totalsTable);
        }

        protected virtual void PrintProducts(/*int vendorId*/ /*Language lang,*/ Font titleFont, Document doc, Order order, Font font, Font attributesFont)
        {
            var productsHeader = new PdfPTable(1)
            {
                //RunDirection = GetDirection(lang),
                WidthPercentage = 100f
            };
            //var cellProducts = GetPdfCell("PDFInvoice.Product(s)", lang, titleFont);
            var cellProducts = GetPdfCell("Danh sách các sản phẩm", titleFont);
            cellProducts.Border = Rectangle.NO_BORDER;
            productsHeader.AddCell(cellProducts);
            doc.Add(productsHeader);
            doc.Add(new Paragraph(" "));

            var orderItems = order.OrderItems;

            //var count = 4 + (_catalogSettings.ShowSkuOnProductDetailsPage ? 1 : 0)
            //            + (_vendorSettings.ShowVendorOnOrderDetailsPage ? 1 : 0);
            var count = 5;

            var productsTable = new PdfPTable(count)
            {
                //RunDirection = GetDirection(lang),
                WidthPercentage = 100f
            };

            var widths = new Dictionary<int, int[]>
            {
                { 4, new[] { 50, 20, 10, 20 } },
                //{ 5, new[] { 45, 15, 15, 10, 15 } },
                { 5, new[] { 40, 15, 10, 15, 20 } },
                { 6, new[] { 40, 13, 13, 12, 10, 12 } }
            };

            productsTable.SetWidths(widths[count]);
            var cellProductItem = GetPdfCell("Tên", font);
            cellProductItem.BackgroundColor = BaseColor.LIGHT_GRAY;
            cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cellProductItem);

            //SKU
            //if (_catalogSettings.ShowSkuOnProductDetailsPage)
            //{
            //    cellProductItem = GetPdfCell("PDFInvoice.SKU", lang, font);
            //    cellProductItem.BackgroundColor = BaseColor.LightGray;
            //    cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
            //    productsTable.AddCell(cellProductItem);
            //}

            //Vendor name
            //if (_vendorSettings.ShowVendorOnOrderDetailsPage)
            //{
            //    cellProductItem = GetPdfCell("PDFInvoice.VendorName", lang, font);
            //    cellProductItem.BackgroundColor = BaseColor.LightGray;
            //    cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
            //    productsTable.AddCell(cellProductItem);
            //}

            //price
            cellProductItem = GetPdfCell("Giá", font);
            cellProductItem.BackgroundColor = BaseColor.LIGHT_GRAY;
            cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cellProductItem);

            //qty
            cellProductItem = GetPdfCell("Số lượng", font);
            cellProductItem.BackgroundColor = BaseColor.LIGHT_GRAY;
            cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cellProductItem);

            cellProductItem = GetPdfCell("Giảm giá", font);
            cellProductItem.BackgroundColor = BaseColor.LIGHT_GRAY;
            cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cellProductItem);

            //total
            cellProductItem = GetPdfCell("Thành tiền", font);
            cellProductItem.BackgroundColor = BaseColor.LIGHT_GRAY;
            cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
            productsTable.AddCell(cellProductItem);

            //var vendors = _vendorSettings.ShowVendorOnOrderDetailsPage ? _vendorService.GetVendorsByIds(orderItems.Select(item => item.Product.VendorId).ToArray()) : new List<Vendor>();

            foreach (var orderItem in orderItems)
            {
                //var p = orderItem.Product;
                var p = orderItem.Item;

                //a vendor should have access only to his products
                //if (vendorId > 0 && p.VendorId != vendorId)
                //    continue;

                var pAttribTable = new PdfPTable(1) { /*RunDirection = GetDirection(lang)*/ };
                pAttribTable.DefaultCell.Border = Rectangle.NO_BORDER;

                //product name
                //var name = _localizationService.GetLocalized(p, x => x.Name, lang.Id);
                var name = p.Name;
                pAttribTable.AddCell(new Paragraph(name, font));
                cellProductItem.AddElement(new Paragraph(name, font));
                //attributes
                //if (!string.IsNullOrEmpty(orderItem.AttributeDescription))
                //{
                //    var attributesParagraph =
                //        new Paragraph(HtmlHelper.ConvertHtmlToPlainText(orderItem.AttributeDescription, true, true),
                //            attributesFont);
                //    pAttribTable.AddCell(attributesParagraph);
                //}

                //rental info
                //if (orderItem.Product.IsRental)
                //{
                //    var rentalStartDate = orderItem.RentalStartDateUtc.HasValue
                //        ? _productService.FormatRentalDate(orderItem.Product, orderItem.RentalStartDateUtc.Value)
                //        : string.Empty;
                //    var rentalEndDate = orderItem.RentalEndDateUtc.HasValue
                //        ? _productService.FormatRentalDate(orderItem.Product, orderItem.RentalEndDateUtc.Value)
                //        : string.Empty;
                //    var rentalInfo = string.Format(_localizationService.GetResource("Order.Rental.FormattedDate"),
                //        rentalStartDate, rentalEndDate);

                //    var rentalInfoParagraph = new Paragraph(rentalInfo, attributesFont);
                //    pAttribTable.AddCell(rentalInfoParagraph);
                //}

                productsTable.AddCell(pAttribTable);

                //SKU
                //if (_catalogSettings.ShowSkuOnProductDetailsPage)
                //{
                //    var sku = _productService.FormatSku(p, orderItem.AttributesXml);
                //    cellProductItem = GetPdfCell(sku ?? string.Empty, font);
                //    cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
                //    productsTable.AddCell(cellProductItem);
                //}

                //Vendor name
                //if (_vendorSettings.ShowVendorOnOrderDetailsPage)
                //{
                //    var vendorName = vendors.FirstOrDefault(v => v.Id == p.VendorId)?.Name ?? string.Empty;
                //    cellProductItem = GetPdfCell(vendorName, font);
                //    cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
                //    productsTable.AddCell(cellProductItem);
                //}

                //price
                string unitPrice;
                //if (order.CustomerTaxDisplayType == TaxDisplayType.IncludingTax)
                //{
                //    //including tax
                //    var unitPriceInclTaxInCustomerCurrency =
                //        _currencyService.ConvertCurrency(orderItem.UnitPriceInclTax, order.CurrencyRate);
                //    unitPrice = _priceFormatter.FormatPrice(unitPriceInclTaxInCustomerCurrency, true,
                //        order.CustomerCurrencyCode, lang, true);
                //}
                //else
                //{
                //    //excluding tax
                //    var unitPriceExclTaxInCustomerCurrency =
                //        _currencyService.ConvertCurrency(orderItem.UnitPriceExclTax, order.CurrencyRate);
                //    unitPrice = _priceFormatter.FormatPrice(unitPriceExclTaxInCustomerCurrency, true,
                //        order.CustomerCurrencyCode, lang, false);
                //}
                unitPrice = CommonFunctions.FormatPrice(p.Price);


                cellProductItem = GetPdfCell(unitPrice, font);
                //cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
                cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
                productsTable.AddCell(cellProductItem);

                //qty
                cellProductItem = GetPdfCell(orderItem.Quantity, font);
                //cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
                cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
                productsTable.AddCell(cellProductItem);

                cellProductItem = GetPdfCell(orderItem.SaleOff, font);
                //cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
                cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
                productsTable.AddCell(cellProductItem);

                //total
                string amount;
                //if (order.CustomerTaxDisplayType == TaxDisplayType.IncludingTax)
                //{
                //    //including tax
                //    var priceInclTaxInCustomerCurrency =
                //        _currencyService.ConvertCurrency(orderItem.PriceInclTax, order.CurrencyRate);
                //    subTotal = _priceFormatter.FormatPrice(priceInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode,
                //        lang, true);
                //}
                //else
                //{
                //    //excluding tax
                //    var priceExclTaxInCustomerCurrency =
                //        _currencyService.ConvertCurrency(orderItem.PriceExclTax, order.CurrencyRate);
                //    subTotal = _priceFormatter.FormatPrice(priceExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode,
                //        lang, false);
                //}

                amount = CommonFunctions.FormatPrice(orderItem.Amount);

                cellProductItem = GetPdfCell(amount, font);
                //cellProductItem.HorizontalAlignment = Element.ALIGN_LEFT;
                cellProductItem.HorizontalAlignment = Element.ALIGN_CENTER;
                productsTable.AddCell(cellProductItem);
            }

            doc.Add(productsTable);
        }

        //protected virtual Paragraph GetParagraph(string resourceKey, string indent, Language lang, Font font, params object[] args)
        protected virtual Paragraph GetParagraph(string text, string indent, Font font, params object[] args)
        {
            //var formatText = _localizationService.GetResource(resourceKey, lang.Id);
            //return new Paragraph(indent + (args.Any() ? string.Format(formatText, args) : formatText), font);
            //var formatText = _localizationService.GetResource(resourceKey, lang.Id);
            return new Paragraph(indent + (args.Any() ? string.Format(text, args) : text), font);
        }

        /// <summary>
        /// Get PDF cell
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="font">Font</param>
        /// <returns>PDF cell</returns>
        protected virtual PdfPCell GetPdfCell(object text, Font font)
        {
            //return new PdfPCell(new Phrase(text.ToString(), font));
            //return text != null ? new PdfPCell(new Phrase(text?.ToString(), font)) : string.Empty;
            return new PdfPCell(new Phrase(text?.ToString(), font));
        }

        /// <summary>
        /// Print header
        /// </summary>
        /// <param name="pdfSettingsByStore">PDF settings</param>
        /// <param name="lang">Language</param>
        /// <param name="order">Order</param>
        /// <param name="font">Text font</param>
        /// <param name="titleFont">Title font</param>
        /// <param name="doc">Document</param>
        //protected virtual void PrintHeader(PdfSettings pdfSettingsByStore, Language lang, Order order, Font font, Font titleFont, Document doc)
        protected virtual void PrintHeader(Order order, Font font, Font titleFont, Document doc)
        {
            //logo
            //var logoPicture = _pictureService.GetPictureById(pdfSettingsByStore.LogoPictureId);
            //var logoExists = logoPicture != null;

            //header
            //var headerTable = new PdfPTable(logoExists ? 2 : 1)
            //{
            //    RunDirection = GetDirection(lang)
            //};
            var headerTable = new PdfPTable(1)
            {
                //RunDirection = GetDirection(lang)
            };
            headerTable.DefaultCell.Border = Rectangle.NO_BORDER;

            //store info
            //var store = _storeService.GetStoreById(order.StoreId) ?? _storeContext.CurrentStore;
            //var anchor = new Anchor(store.Url.Trim('/'), font)
            //{
            //    Reference = store.Url
            //};

            //var cellHeader = GetPdfCell(string.Format(_localizationService.GetResource("PDFInvoice.Order#", lang.Id), order.CustomOrderNumber), titleFont);
            //var cellHeader = GetPdfCell(string.Format(_localizationService.GetResource("PDFInvoice.Order#"), order.Id), titleFont);
            //var cellHeader = GetPdfCell(string.Format(_localizationService.GetResource("PDFInvoice.Order#"), order.Id), titleFont);

            //var cellHeader = new PdfPCell(new Phrase($"Mã đơn hàng: {order.Id}", font));
            var cellHeader = new PdfPCell(GetParagraph("Thông tin đơn hàng", string.Empty, titleFont));
            cellHeader.Phrase.Add(new Phrase(Environment.NewLine));
            cellHeader.Phrase.Add(new Phrase(Environment.NewLine));
            //cellHeader.Phrase.Add(new Phrase($"Mã đơn hàng: {order.Id}", font));
            cellHeader.Phrase.Add(GetParagraph("Mã đơn hàng: {0}", string.Empty, font, order.Id));
            cellHeader.Phrase.Add(new Phrase(Environment.NewLine));
            //cellHeader.Phrase.Add(new Phrase(anchor));
            //cellHeader.Phrase.Add(new Phrase($"Trạng thái đơn hàng: {order.Status.GetDisplayName()}", font));
            cellHeader.Phrase.Add(GetParagraph("Trạng thái đơn hàng: {0}", string.Empty, font, order.Status.GetDisplayName()));
            cellHeader.Phrase.Add(new Phrase(Environment.NewLine));
            //cellHeader.Phrase.Add(GetParagraph("PDFInvoice.OrderDate", lang, font, _dateTimeHelper.ConvertToUserTime(order.CreatedOnUtc, DateTimeKind.Utc).ToString("D", new CultureInfo(lang.LanguageCulture))));
            cellHeader.Phrase.Add(GetParagraph("Ngày đặt hàng: {0}", string.Empty, font, CommonFunctions.FormatDateTime(order.OrderDate)));
            cellHeader.Phrase.Add(new Phrase(Environment.NewLine));
            cellHeader.Phrase.Add(new Phrase(Environment.NewLine));
            cellHeader.HorizontalAlignment = Element.ALIGN_LEFT;
            cellHeader.Border = Rectangle.NO_BORDER;

            headerTable.AddCell(cellHeader);

            //if (logoExists)
            //    headerTable.SetWidths(lang.Rtl ? new[] { 0.2f, 0.8f } : new[] { 0.8f, 0.2f });
            headerTable.WidthPercentage = 100f;

            //logo               
            //if (logoExists)
            //{
            //    var logoFilePath = _pictureService.GetThumbLocalPath(logoPicture, 0, false);
            //    var logo = Image.GetInstance(logoFilePath);
            //    logo.Alignment = GetAlignment(lang, true);
            //    logo.ScaleToFit(65f, 65f);

            //    var cellLogo = new PdfPCell { Border = Rectangle.NO_BORDER };
            //    cellLogo.AddElement(logo);
            //    headerTable.AddCell(cellLogo);
            //}

            doc.Add(headerTable);
        }

        /// <summary>
        /// Print addresses
        /// </summary>
        /// <param name="vendorId">Vendor identifier</param>
        /// <param name="lang">Language</param>
        /// <param name="titleFont">Title font</param>
        /// <param name="order">Order</param>
        /// <param name="font">Text font</param>
        /// <param name="doc">Document</param>
        //protected virtual void PrintAddresses(int vendorId, Font titleFont, Order order, Font font, Document doc)
        protected virtual void PrintAddresses(Font titleFont, Order order, Font font, Document doc)
        {
            //var addressTable = new PdfPTable(2) { RunDirection = GetDirection(lang) };
            //var addressTable = new PdfPTable(2);
            var addressTable = new PdfPTable(2);
            addressTable.DefaultCell.Border = Rectangle.NO_BORDER;
            addressTable.WidthPercentage = 100f;
            addressTable.SetWidths(new[] { 50, 50 });

            //billing info
            //PrintBillingInfo(vendorId, titleFont, order, font, addressTable);
            //PrintBillingInfo(titleFont, order, font, addressTable);

            //shipping info
            //PrintShippingInfo(order, titleFont, font, addressTable);

            doc.Add(addressTable);
            doc.Add(new Paragraph(" "));
        }

        /// <summary>
        /// Print billing info
        /// </summary>
        /// <param name="vendorId">Vendor identifier</param>
        /// <param name="lang">Language</param>
        /// <param name="titleFont">Title font</param>
        /// <param name="order">Order</param>
        /// <param name="font">Text font</param>
        /// <param name="addressTable">Address PDF table</param>
        //protected virtual void PrintBillingInfo(int vendorId, Font titleFont, Order order, Font font, PdfPTable addressTable)
        //protected virtual void PrintBillingInfo(int vendorId, Font titleFont, Order order, Font font, PdfPTable addressTable)
        //protected virtual void PrintBillingInfo(Font titleFont, Order order, Font font, PdfPTable addressTable)
        protected virtual void PrintBillingInfo(Font titleFont, Order order, Font font, Document doc)
        {
            //const string indent = "   ";
            //string indent = string.Empty;
            //var billingAddress = new PdfPTable(1) { RunDirection = GetDirection(lang) };
            var billingAddress = new PdfPTable(1);
            billingAddress.DefaultCell.Border = Rectangle.NO_BORDER;

            //billingAddress.AddCell(GetParagraph("PDFInvoice.BillingInformation", lang, titleFont));
            //var cellBilling = new PdfPCell(new Phrase($"Thông tin nhận hàng", font));
            //cellBilling.Phrase.Add(new Phrase(Environment.NewLine));
            //billingAddress.AddCell(cellBilling);
            //cellBilling.Phrase.Add(new Phrase($"Thông tin nhận hàng", font));
            //cellBilling.Phrase.Add(new Phrase(Environment.NewLine));
            //billingAddress.AddCell(cellBilling);
            //cellBilling.Phrase.Add(new Phrase($"Thông tin nhận hàng", font));
            //cellBilling.Phrase.Add(new Phrase(Environment.NewLine));
            var cellBilling = new PdfPCell(GetParagraph("Thông tin người nhận hàng", string.Empty, titleFont));
            cellBilling.Phrase.Add(new Phrase(Environment.NewLine));
            cellBilling.Phrase.Add(new Phrase(Environment.NewLine));
            //billingAddress.AddCell(GetParagraph("Thông tin người nhận hàng", indent, titleFont));
            //billingAddress.AddCell(cellBilling);
            //if (_addressSettings.CompanyEnabled && !string.IsNullOrEmpty(order.BillingAddress.Company))
            //    billingAddress.AddCell(GetParagraph("PDFInvoice.Company", indent, lang, font, order.BillingAddress.Company));

            //billingAddress.AddCell(GetParagraph("PDFInvoice.Name", indent, lang, font, order.BillingAddress.FirstName + " " + order.BillingAddress.LastName));
            //billingAddress.AddCell(GetParagraph("PDFInvoice.Name", indent, lang, font, order.BillingAddress.FirstName + " " + order.BillingAddress.LastName));
            //cellHeader.Phrase.Add(GetParagraph("Mã đơn hàng: {0}", string.Empty, font, order.Id));
            cellBilling.Phrase.Add(GetParagraph("Tên người nhận: {0}", string.Empty, font, order.Address.RecipientName));
            cellBilling.Phrase.Add(new Phrase(Environment.NewLine));
            //if (_addressSettings.PhoneEnabled)
            //billingAddress.AddCell(GetParagraph("Số điện thoại: {0}", indent, font, order.Customer.PhoneNumber));
            cellBilling.Phrase.Add(GetParagraph("Số điện thoại: {0}", string.Empty, font, order.Address.PhoneNumber));
            cellBilling.Phrase.Add(new Phrase(Environment.NewLine));
            //if (_addressSettings.FaxEnabled && !string.IsNullOrEmpty(order.BillingAddress.FaxNumber))
            //    billingAddress.AddCell(GetParagraph("PDFInvoice.Fax", indent, lang, font, order.BillingAddress.FaxNumber));
            //if (_addressSettings.StreetAddressEnabled)
            //    billingAddress.AddCell(GetParagraph("PDFInvoice.Address", indent, lang, font, order.BillingAddress.Address1));
            //billingAddress.AddCell(GetParagraph("Địa chỉ: {0}", indent, font, order.Address.Detail));
            if (order.Address != null)
            {
                var detail = !string.IsNullOrEmpty(order.Address.Detail) ? $"{order.Address.Detail}, " : "";
                var ward = !string.IsNullOrEmpty(order.Address.Ward) ? $"{order.Address.Ward}, " : "";
                var district = !string.IsNullOrEmpty(order.Address.District) ? $"{order.Address.District}, " : "";
                var province = !string.IsNullOrEmpty(order.Address.Province) ? $"{order.Address.Province}" : "";
                cellBilling.Phrase.Add(GetParagraph("Địa chỉ: {0}", string.Empty, font, $"{detail}{ward}{district}{province}"));
                cellBilling.Phrase.Add(new Phrase(Environment.NewLine));
            }

            //if (_addressSettings.StreetAddress2Enabled && !string.IsNullOrEmpty(order.BillingAddress.Address2))
            //    billingAddress.AddCell(GetParagraph("PDFInvoice.Address2", indent, lang, font, order.BillingAddress.Address2));
            //if (_addressSettings.CityEnabled || _addressSettings.StateProvinceEnabled ||
            //    _addressSettings.CountyEnabled || _addressSettings.ZipPostalCodeEnabled)
            //{
            //    var addressLine = $"{indent}{order.BillingAddress.City}, " +
            //        $"{(!string.IsNullOrEmpty(order.BillingAddress.County) ? $"{order.BillingAddress.County}, " : string.Empty)}" +
            //        $"{(order.BillingAddress.StateProvince != null ? _localizationService.GetLocalized(order.BillingAddress.StateProvince, x => x.Name, lang.Id) : string.Empty)} " +
            //        $"{order.BillingAddress.ZipPostalCode}";
            //    billingAddress.AddCell(new Paragraph(addressLine, font));
            //}

            //if (_addressSettings.CountryEnabled && order.BillingAddress.Country != null)
            //    billingAddress.AddCell(new Paragraph(indent + _localizationService.GetLocalized(order.BillingAddress.Country, x => x.Name, lang.Id),
            //        font));

            //VAT number
            //if (!string.IsNullOrEmpty(order.VatNumber))
            //    billingAddress.AddCell(GetParagraph("PDFInvoice.VATNumber", indent, lang, font, order.VatNumber));

            //custom attributes
            //var customBillingAddressAttributes =
            //    _addressAttributeFormatter.FormatAttributes(order.BillingAddress.CustomAttributes);
            //if (!string.IsNullOrEmpty(customBillingAddressAttributes))
            //{
            //    //TODO: we should add padding to each line (in case if we have several custom address attributes)
            //    billingAddress.AddCell(
            //        new Paragraph(indent + HtmlHelper.ConvertHtmlToPlainText(customBillingAddressAttributes, true, true), font));
            //}

            //vendors payment details
            //if (vendorId == 0)
            //{
            //    //payment method
            //    var paymentMethod = _paymentService.LoadPaymentMethodBySystemName(order.PaymentMethodSystemName);
            //    var paymentMethodStr = paymentMethod != null
            //        ? _localizationService.GetLocalizedFriendlyName(paymentMethod, lang.Id)
            //        : order.PaymentMethodSystemName;
            //    if (!string.IsNullOrEmpty(paymentMethodStr))
            //    {
            //        billingAddress.AddCell(new Paragraph(" "));
            //        billingAddress.AddCell(GetParagraph("PDFInvoice.PaymentMethod", indent, lang, font, paymentMethodStr));
            //        billingAddress.AddCell(new Paragraph());
            //    }

            //    //custom values
            //    var customValues = _paymentService.DeserializeCustomValues(order);
            //    if (customValues != null)
            //    {
            //        foreach (var item in customValues)
            //        {
            //            billingAddress.AddCell(new Paragraph(" "));
            //            billingAddress.AddCell(new Paragraph(indent + item.Key + ": " + item.Value, font));
            //            billingAddress.AddCell(new Paragraph());
            //        }
            //    }
            //}
            //billingAddress.AddCell(new Paragraph(" "));
            //billingAddress.AddCell(GetParagraph("Phương thức thanh toán: {0}", indent, font, order.PaymentType.GetDisplayName()));
            cellBilling.Phrase.Add(GetParagraph("Phương thức thanh toán: {0}", string.Empty, font, order.PaymentType.GetDisplayName()));

            //billingAddress.AddCell(new Paragraph(" "));
            //billingAddress.AddCell(new Paragraph(" "));
            cellBilling.Phrase.Add(new Phrase(Environment.NewLine));
            cellBilling.Phrase.Add(GetParagraph("Phương thức nhận hàng: {0}", string.Empty, font, order.ReceivingType.Name));
            cellBilling.Phrase.Add(new Phrase(Environment.NewLine));
            cellBilling.Phrase.Add(new Phrase(Environment.NewLine));
            cellBilling.HorizontalAlignment = Element.ALIGN_LEFT;
            cellBilling.Border = Rectangle.NO_BORDER;
            billingAddress.AddCell(cellBilling);
            billingAddress.WidthPercentage = 100f;
            //addressTable.AddCell(billingAddress);
            doc.Add(billingAddress);
        }
    }
}