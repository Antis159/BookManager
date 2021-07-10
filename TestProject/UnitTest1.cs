using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookManager;
using System.Collections.Generic;
using System;

namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AddAndDeleteBookTest()
        {
            Library lib = new Library();
            Library.Book book = new Library.Book();
            book.name = "Rick's Adventures";
            book.author = "Rick P.";
            book.category = "Adventure";
            book.language = "English";
            book.publicationDate = DateTime.Now;
            book.isbn = "135-351-3131";
            book.isTaken = false;

            List<Library.Book> tempBooks = new List<Library.Book>();
            lib.SaveBooks(tempBooks);
            lib.AddBook(book);
            tempBooks = lib.LoadBooks();
            bool success = false;
            foreach (Library.Book item in tempBooks)
            {
                if (item.isbn == book.isbn)
                {
                    lib.DeleteBook(0);
                    success = true;
                    break;
                }
            }
            Assert.IsTrue(success);
        }

        [TestMethod]
        public void FilterListTest()
        {
            Library lib = new Library();
            Library.Book book1 = new Library.Book();
            Library.Book book2 = new Library.Book();
            book1.name = "b";
            book2.name = "a";
            List<Library.Book> tempBooks = new List<Library.Book>();
            lib.SaveBooks(tempBooks);
            lib.AddBook(book1);
            lib.AddBook(book2);
            lib.FilterBookByCategory("name");
            tempBooks = lib.LoadBooks();
            bool success = false;
            if (tempBooks[0].name == book2.name && tempBooks[1].name == book1.name)
                success = true;
            tempBooks.Clear();
            lib.SaveBooks(tempBooks);
            Assert.IsTrue(success);
        }

        [TestMethod]
        public void TakeBookTest()
        {
            Library lib = new Library(); 
            List<Library.Book> tempBooks = new List<Library.Book>();
            lib.SaveBooks(tempBooks);
            for (int i = 0; i < 4; i++)
                lib.AddBook(new Library.Book());
            tempBooks = lib.LoadBooks();
            lib.SaveBooks(tempBooks);
            bool success = false;
            lib.TakeBook("Vilius", 3, 0);
            tempBooks = lib.LoadBooks();
            if (!tempBooks[0].isTaken) //cannot take book for more than 2 months
                success = true;
            else
                Assert.Fail();

            lib.TakeBook("Vilius", 2, 0); //take book according to rules
            tempBooks = lib.LoadBooks();
            if (tempBooks[0].isTaken)
                success = true;
            else
                Assert.Fail();

            lib.TakeBook("Vilius", 2, 1);
            lib.TakeBook("Vilius", 2, 2);
            lib.TakeBook("Vilius", 2, 3);
            tempBooks = lib.LoadBooks();
            if (!tempBooks[3].isTaken) //cannot take more than 3 books on one name
                success = true;
            else
                Assert.Fail();

            Assert.IsTrue(success);
            tempBooks.Clear();
            lib.SaveBooks(tempBooks);
        }
        [TestMethod]
        public void ReturnBookTest()
        {
            Library lib = new Library();
            List<Library.Book> tempBooks = new List<Library.Book>();
            lib.SaveBooks(tempBooks);
            lib.AddBook(new Library.Book());
            lib.TakeBook("Vilius", 2, 0);
            lib.ReturnBook(0);
            tempBooks = lib.LoadBooks();
            bool success = false;
            if (!tempBooks[0].isTaken)
                success = true;
            Assert.IsTrue(success);
        }
    }
}
