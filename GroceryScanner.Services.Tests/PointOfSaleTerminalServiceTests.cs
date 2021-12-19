using GroceryScanner.Core.Services;
using GroceryScanner.Core.Services.Products.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace GroceryScanner.Services.Tests
{
    [TestClass]
    public class PointOfSaleTerminalServiceTests
    {
        private IPointOfSaleTerminalService GetService()
        {
            return new PointOfSaleTerminalService();
        }

        //[TestInitialize]
        //public void SetupTestEnvironment()
        //{
        //    var terminal = GetService();
        //    terminal.SetPricing();
        //}

        [TestCleanup]
        public void CleanupTestEnvironment()
        {
            ProductList.Products.Clear();
            SalesOrder.Products.Clear();
        }

        [TestMethod]
        public void SetPrices_SetsProductList()
        {
            // Arrange
            var terminal = GetService();

            // Act
            terminal.SetPricing();

            //Assert
            Assert.IsNotNull(ProductList.Products);
        }

        [TestMethod]
        public void ScanProduct_ProductA_AddsProductToOrder()
        {
            // Arrange
            var terminal = GetService();
            terminal.SetPricing();

            // Act
            terminal.ScanProduct("A");

            //Assert
            Assert.IsNotNull(SalesOrder.Products);
            Assert.IsTrue(SalesOrder.Products[0].Code == "A");
        }

        [TestMethod]
        public void ScanProduct_MultipleProducts_AddsProductsToOrder()
        {
            // Arrange
            var terminal = GetService();
            terminal.SetPricing();

            // Act
            terminal.ScanProduct("A");
            terminal.ScanProduct("C");
            terminal.ScanProduct("D");

            //Assert
            Assert.IsNotNull(SalesOrder.Products);
            Assert.AreEqual(SalesOrder.Products.Count(), 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ScanProduct_InvalidProductCode_ThrowsException()
        {
            // Arrange
            var terminal = GetService();
            terminal.SetPricing();

            // Act
            terminal.ScanProduct("Z");
        }

        [TestMethod]
        public void CalculateTotal_Scenario1_ReturnsCorrectTotal()
        {
            // Arrange
            var terminal = GetService();
            terminal.SetPricing();

            // Act
            terminal.ScanProduct("A");
            terminal.ScanProduct("B");
            terminal.ScanProduct("C");
            terminal.ScanProduct("D");
            terminal.ScanProduct("A");
            terminal.ScanProduct("B");
            terminal.ScanProduct("A");
            var total = terminal.CalculateTotal();

            //Assert
            Assert.IsNotNull(SalesOrder.Products);
            Assert.AreEqual(SalesOrder.Products.Count(), 7);
            Assert.IsTrue(total > 0);
            Assert.AreEqual(total, 13.25M);
        }

        [TestMethod]
        public void CalculateTotal_Scenario2_ReturnsCorrectTotal()
        {
            // Arrange
            var terminal = GetService();
            terminal.SetPricing();

            // Act
            terminal.ScanProduct("C");
            terminal.ScanProduct("C");
            terminal.ScanProduct("C");
            terminal.ScanProduct("C");
            terminal.ScanProduct("C");
            terminal.ScanProduct("C");
            terminal.ScanProduct("C");
            var total = terminal.CalculateTotal();

            //Assert
            Assert.IsNotNull(SalesOrder.Products);
            Assert.AreEqual(SalesOrder.Products.Count(), 7);
            Assert.IsTrue(total > 0);
            Assert.AreEqual(total, 6.00M);
        }

        [TestMethod]
        public void CalculateTotal_Scenario3_ReturnsCorrectTotal()
        {
            // Arrange
            var terminal = GetService();
            terminal.SetPricing();

            // Act
            terminal.ScanProduct("A");
            terminal.ScanProduct("B");
            terminal.ScanProduct("C");
            terminal.ScanProduct("D");
            var total = terminal.CalculateTotal();

            //Assert
            Assert.IsNotNull(SalesOrder.Products);
            Assert.AreEqual(SalesOrder.Products.Count(), 4);
            Assert.IsTrue(total > 0);
            Assert.AreEqual(total, 7.25M);
        }

        [TestMethod]
        public void CalculateTotal_MultiBuyAndStandardPriceMix_ReturnsCorrectTotal()
        {
            // Arrange
            var terminal = GetService();
            terminal.SetPricing();

            // Act
            terminal.ScanProduct("A");
            terminal.ScanProduct("A");
            terminal.ScanProduct("A"); // $3
            terminal.ScanProduct("A");
            terminal.ScanProduct("A");
            terminal.ScanProduct("A"); // $6
            terminal.ScanProduct("A");
            terminal.ScanProduct("A"); // $8.50
            terminal.ScanProduct("C");
            terminal.ScanProduct("C");
            terminal.ScanProduct("C");
            terminal.ScanProduct("C");
            terminal.ScanProduct("C");
            terminal.ScanProduct("C"); // $13.50
            terminal.ScanProduct("C");
            terminal.ScanProduct("C");
            terminal.ScanProduct("C");
            terminal.ScanProduct("C");
            terminal.ScanProduct("C");
            terminal.ScanProduct("C"); // $18.50
            terminal.ScanProduct("C");
            terminal.ScanProduct("C"); // $20.50
            var total = terminal.CalculateTotal();

            //Assert
            Assert.IsNotNull(SalesOrder.Products);
            Assert.AreEqual(SalesOrder.Products.Count(), 22);
            Assert.IsTrue(total > 0);
            Assert.AreEqual(total, 20.50M);
        }
    }
}