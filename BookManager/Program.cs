using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace BookManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Library lib = new Library();
            lib.LoadBooks();
            while(true)
            {
                Console.WriteLine("--------------------------------");
                Console.WriteLine("List of all commands: ");
                Console.WriteLine("addBook");
                Console.WriteLine("deleteBook");
                Console.WriteLine("takeBook");
                Console.WriteLine("returnBook");
                Console.WriteLine("showList");
                Console.WriteLine("exit");
                Console.WriteLine("--------------------------------");

                string input = Console.ReadLine();

                if (input == "addBook")
                    lib.AddBook();
                if (input == "deleteBook")
                    lib.DeleteBook();
                if (input == "takeBook")
                    lib.TakeBook();
                if (input == "returnBook")
                    lib.ReturnBook();
                if (input == "showList")
                    lib.FilterBookByCategory();
                if (input == "exit")
                    Environment.Exit(0);

                if(input == "asd")
                {
                    Library.Book book = new Library.Book();
                    book.name = "Rick's Adventures";
                    book.author = "Rick P.";
                    book.category = "Adventure";
                    book.language = "English";
                    book.publicationDate = DateTime.Now;
                    book.isbn = "135-351-3131";
                    book.isTaken = false;

                    lib.AddBook(book);


                    lib.DeleteBook(0);
                }
            }
        }

    }
    public class Library
    {
        public class Book
        {
            public string name { get; set; }
            public string author { get; set; }
            public string category { get; set; }
            public string language { get; set; }
            public DateTime publicationDate { get; set; }
            public string isbn { get; set; }
            public bool isTaken { get; set; }
            public DateTime takenDate { get; set; }
            public string takenName { get; set; }
            public int takenMonths { get; set; }
        }

        //Sorting classes
        #region
        public class SortByName : IComparer<Book>
        {
            public int Compare(Book x, Book y)
            {
                return x.name.CompareTo(y.name);
            }
        }
        public class SortByAuthor : IComparer<Book>
        {
            public int Compare(Book x, Book y)
            {
                return x.author.CompareTo(y.author);
            }
        }
        public class SortByCategory : IComparer<Book>
        {
            public int Compare(Book x, Book y)
            {
                return x.category.CompareTo(y.category);
            }
        }
        public class SortByLanguage : IComparer<Book>
        {
            public int Compare(Book x, Book y)
            {
                return x.language.CompareTo(y.language);
            }
        }
        public class SortByPublicationDate : IComparer<Book>
        {
            public int Compare(Book x, Book y)
            {
                return x.publicationDate.CompareTo(y.publicationDate);
            }
        }
        public class SortByISBN : IComparer<Book>
        {
            public int Compare(Book x, Book y)
            {
                return x.isbn.CompareTo(y.isbn);
            }
        }
        public class SortByAvailability : IComparer<Book>
        {
            public int Compare(Book x, Book y)
            {
                return x.isTaken.CompareTo(y.isTaken);
            }
        }
        #endregion

        public List<Book> LoadBooks()
        {
            List<Book> tempBooks = new List<Book>();
            if (File.Exists(@"books.json"))
                tempBooks = JsonConvert.DeserializeObject<List<Book>>(File.ReadAllText(@"books.json"));
            return tempBooks;
        }

        public void SaveBooks(List<Book> books)
        {
            string stringJson = JsonConvert.SerializeObject(books);
            File.WriteAllText(@"books.json", stringJson);
        }

        public void AddBook(Book tempBook = null)
        {
            if (tempBook == null)
            {
                tempBook = new Book();
                Console.WriteLine("Enter book name: ");
                tempBook.name = Console.ReadLine();
                Console.WriteLine("Enter book author: ");
                tempBook.author = Console.ReadLine();
                Console.WriteLine("Enter book category: ");
                tempBook.category = Console.ReadLine();
                Console.WriteLine("Enter book language: ");
                tempBook.language = Console.ReadLine();
                Console.WriteLine("Enter book publishing date (dd/mm/yyyy): ");
                string tempDate = Console.ReadLine();
                Regex reg = new Regex(@"^(\d){2}/(\d){2}/(\d){4}$");
                Match x = reg.Match(tempDate);
                if (!x.Success)
                {
                    Console.WriteLine("Wrong date format entered");
                    return;
                }
                tempBook.publicationDate = Convert.ToDateTime(tempDate);
                Console.WriteLine("Enter book ISBN (International Standart Book Number): ");
                tempBook.isbn = Console.ReadLine();
                tempBook.isTaken = false;
            }

            List<Book> tempBooks = LoadBooks();
            if (tempBook == null)
                return;
            if(tempBooks == null)
                tempBooks = new List<Book>() { tempBook};
            else
                tempBooks.Add(tempBook);
            SaveBooks(tempBooks);
            Console.WriteLine($"The book {tempBook.name} has been added");

            return;
        }

        public void DeleteBook(int index = int.MaxValue)
        {
            if(index == int.MaxValue)
            {
                Console.WriteLine("Enter book number to delete");
                if (!int.TryParse(Console.ReadLine(), out index))
                {
                    Console.WriteLine($"A number was not entered");
                    return;
                }
            }
            List<Book> tempBooks = LoadBooks();
            if(index < tempBooks.Count)
            {
                tempBooks.Remove(tempBooks[index]);
                SaveBooks(tempBooks);
            }
            else
            {
                Console.WriteLine("No book with entered number");
                return;
            }
            Console.WriteLine("The selected book has been deleted");
            return;
        }

        public void FilterBookByCategory(string input = null)
        {
            if(input == null)
            {
                Console.WriteLine("Enter book filter method");
                Console.WriteLine("name / author / category / language / publication / ISBN / availability");
                input = Console.ReadLine();
            }
            List<Book> tempBooks = LoadBooks();
            //Sort Methods
            #region
            if (input == "name")
            {
                SortByName sort = new SortByName();
                tempBooks.Sort(sort);
            }
            if(input == "author")
            {
                SortByAuthor sort = new SortByAuthor();
                tempBooks.Sort(sort);
            }
            if (input == "category")
            {
                SortByCategory sort = new SortByCategory();
                tempBooks.Sort(sort);
            }
            if (input == "language")
            {
                SortByLanguage sort = new SortByLanguage();
                tempBooks.Sort(sort);
            }
            if (input == "publication")
            {
                SortByPublicationDate sort = new SortByPublicationDate();
                tempBooks.Sort(sort);
            }
            if (input == "ISBN")
            {
                SortByISBN sort = new SortByISBN();
                tempBooks.Sort(sort);
            }
            if (input == "availability")
            {
                SortByAvailability sort = new SortByAvailability();
                tempBooks.Sort(sort);
            }
            SaveBooks(tempBooks);
            #endregion // //Sort Methods
            Console.WriteLine("List of all books (Number / Name / Author / Category / Language / Publication Date / ISBN)");
            for (int i = 0; i < tempBooks.Count; i++)
            {
                string bookAvailability;
                if (tempBooks[i].isTaken)
                    bookAvailability = "Taken";
                else
                    bookAvailability = "Available";

                Console.WriteLine($"{i} / {tempBooks[i].name} / {tempBooks[i].author} / {tempBooks[i].category} / " +
                                  $"{tempBooks[i].language} / {tempBooks[i].publicationDate.ToString("dd/MM/yyyy")} / {tempBooks[i].isbn} / {bookAvailability}");
            }

            return;
        }

        public void TakeBook(string name = null, int nMonths = int.MaxValue, int index = int.MaxValue)
        {
            if(name == null)
            {
                Console.WriteLine("Specify who is taking the books");
                name = Console.ReadLine();
            }
            if(!CheckAllowedBooksCount(name))
            {
                Console.WriteLine("The maximum amount of books have already been taken on that name");
                return;
            }
            if(nMonths == int.MaxValue)
            {
                Console.WriteLine("Specify for what period the books is needed in number of months");
                if (!int.TryParse(Console.ReadLine(), out nMonths))
                {
                    Console.WriteLine("A number was not entered");
                    return;
                }
            }
            if (nMonths > 2)
            {
                Console.WriteLine("Cannot take books for more than 2 months");
                return;
            }
            if(index == int.MaxValue)
            {
                Console.WriteLine("Enter book number to take");
                if (!int.TryParse(Console.ReadLine(), out index))
                {
                    Console.WriteLine($"A number was not entered");
                    return;
                }
            }
            List<Book> tempBooks = LoadBooks();
            if (index < tempBooks.Count)
            {
                if (!tempBooks[index].isTaken)
                {
                    Book tempBook = tempBooks[index];
                    tempBook.isTaken = true;
                    tempBook.takenName = name;
                    tempBook.takenDate = DateTime.Now;
                    tempBook.takenMonths = nMonths;
                    tempBooks[index] = tempBook;
                    SaveBooks(tempBooks);
                    Console.WriteLine($"You have taken the book: {tempBooks[index].name} ");
                }
                else
                {
                    Console.WriteLine("That book is already taken");
                    return;
                }
            }
            else
            {
                Console.WriteLine($"There is no book with the {index} number");
                return;
            }
            return;
        }

        private bool CheckAllowedBooksCount(string name)
        {
            int allowedBooksPerPerson = 3;
            int counter = 0;
            List<Book> tempBooks = LoadBooks();
            for(int i=0; i< tempBooks.Count; i++)
            {
                if (tempBooks[i].takenName == name)
                    counter += 1;
                if (counter >= allowedBooksPerPerson)
                    return false;
            }

            return true;
        }

        public void ReturnBook(int index = int.MaxValue)
        {
            if(index == int.MaxValue)
            {
                Console.WriteLine("Enter book number to return");
                if (!int.TryParse(Console.ReadLine(), out index))
                {
                    Console.WriteLine($"A number was not entered");
                    return;
                }
            }
            List<Book> tempBooks = LoadBooks();
            if (index < tempBooks.Count)
            {
                if (tempBooks[index].isTaken)
                {
                    Book tempBook = tempBooks[index];
                    tempBook.isTaken = false;
                    tempBooks[index] = tempBook;
                    SaveBooks(tempBooks);
                    Console.WriteLine($"You have returned the book: {tempBooks[index].name} ");
                    if (tempBook.takenDate.AddMonths(tempBook.takenMonths) < DateTime.Now)
                        Console.WriteLine("You were late in returning, this library holds grudges");
                }
                else
                {
                    Console.WriteLine("That book is not taken");
                    return;
                }
            }
            else
            {
                Console.WriteLine($"There is no book with the {index} number");
                return;
            }

            return;
        }
    }
}
