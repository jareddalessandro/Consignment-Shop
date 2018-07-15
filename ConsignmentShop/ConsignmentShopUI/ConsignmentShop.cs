using ConsignmentShopLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsignmentShopUI
{
    public partial class ConsignmentShop : Form
    {
        private Store store = new Store();
        private List<Item> shoppingCartData = new List<Item>();
        BindingSource itemsBinding = new BindingSource();
        BindingSource cartBinding = new BindingSource();
        BindingSource vendorsBinding = new BindingSource();        
        private decimal storeProfit = 0;
        private decimal totalPrice = 0;


        public ConsignmentShop()
        {
            InitializeComponent();
            SetupData();
            
            // for using a sold property instead of a quantity
            //itemsBinding.DataSource = store.Items.Where(x => x.Sold == false).ToList();
            itemsBinding.DataSource = store.Items;
            itemsListbox.DataSource = itemsBinding;
            itemsListbox.DisplayMember = "Display";
            itemsListbox.ValueMember = "Display";

            cartBinding.DataSource = shoppingCartData;
            shoppingCartListbox.DataSource = cartBinding;
            shoppingCartListbox.DisplayMember = "Display";
            shoppingCartListbox.ValueMember = "Display";

            vendorsBinding.DataSource = store.Vendors;
            vendorListbox.DataSource = vendorsBinding;
            vendorListbox.DisplayMember = "Display";
            vendorListbox.ValueMember = "Display";                       
        }

        private void SetupData()
        {
            // The commission is hard coded in the vendor.cs file
            store.Vendors.Add(new Vendor { FirstName = "Jim", LastName = "Halpert" }); //[0]
            store.Vendors.Add(new Vendor { FirstName = "Dwight", LastName = "Shrute", }); //[1]
            store.Vendors.Add(new Vendor { FirstName = "Stanley", LastName = "Hudson", });
            store.Vendors.Add(new Vendor { FirstName = "Creed", LastName = "Bratton", });


            store.Items.Add(new Item
            {
                Package = "12 Reams",
                Price = 9.25M,
                Sheets = "6,000",
                Quantity = 16,
                Owner = store.Vendors[0]
            });

            store.Items.Add(new Item
            {
                Package = "10 Reams",                
                Price = 7.25M,
                Sheets = "5,000",
                Quantity = 14,
                Owner = store.Vendors[0]
            });

            store.Items.Add(new Item
            {
                Package = "8 Reams",
                Price = 6.00M,
                Sheets = "4,000",
                Quantity = 20,
                Owner = store.Vendors[1]
            });

            store.Items.Add(new Item
            {
                Package = "5 Reams",
                Price = 4.50M,
                Sheets = "2,500",
                Quantity = 25,
                Owner = store.Vendors[1]
            });

            store.Items.Add(new Item
            {
                Package = "2 Reams",                
                Price = 1.75M,
                Sheets = "1,000",
                Quantity = 29,
                Owner = store.Vendors[2]
            });

            store.Items.Add(new Item
            {
                Package = "1 Reams",
                Price = 1.00M,
                Sheets = "500",
                Quantity = 30,
                Owner = store.Vendors[2]
            });

            store.Items.Add(new Item
            {
                Package = "Colored Toner",
                Price = 5.00M,
                Quantity = 15,
                Owner = store.Vendors[0]
            });

            store.Items.Add(new Item
            {
                Package = "Magenta Toner",
                Price = 2.00M,
                Quantity = 9,
                Owner = store.Vendors[1]
            });

            store.Items.Add(new Item
            {
                Package = "Black Toner",
                Price = 2.00M,
                Quantity = 30,
                Owner = store.Vendors[2]
            });

            store.Items.Add(new Item
            {
                Package = "48inch Stake",
                Price = 5.00M,
                Quantity = 2,
                Owner = store.Vendors[3]
            });

            store.Items.Add(new Item
            {
                Package = "60inch Stake",
                Price = 6.00M,
                Quantity = 2,
                Owner = store.Vendors[3]
            });


            store.Name = "Dunder Mifflin";
        }

        private void addToCart_Click(object sender, EventArgs e)
        {
            // Figure out what is selected from the items list
            // Copy that item to the shopping cart 
            //Figures out total price every time addToCart is used
            totalPrice = 0;
            Item selectedItem = (Item)itemsListbox.SelectedItem;
            selectedItem.Quantity -= 1;
            if (selectedItem.Quantity == 0)
            {
                store.Items.Remove(selectedItem);
            }

            shoppingCartData.Add(selectedItem);
            foreach (Item item in shoppingCartData)
            {
                totalPrice += item.Price;
            }
            totalPriceValue.Text = string.Format("${0}", totalPrice);

            itemsBinding.ResetBindings(false);
            cartBinding.ResetBindings(false);
        }

        private void pullFromCart_Click(object sender, EventArgs e)
        {
            //Figures out total price every time pullFromCart is used
            //Remove the selected item from the shopping cart
            totalPrice = 0;
            Item selectedItem = (Item)shoppingCartListbox.SelectedItem;
            if (selectedItem.Quantity == 0)
            {
                store.Items.Add(selectedItem);
            }
            selectedItem.Quantity += 1;

            shoppingCartData.Remove(selectedItem);
            foreach (Item item in shoppingCartData)
            {
                totalPrice += item.Price;
            }
            totalPriceValue.Text = string.Format("${0}", totalPrice);

            itemsBinding.ResetBindings(false);
            cartBinding.ResetBindings(false);

        }

        private void makePurchase_Click(object sender, EventArgs e)
        {
            // Clear the cart
            //The store profit and vendor incomes continues to accrue through multiple purchases            
                        
            foreach (Item item in shoppingCartData)
            {
                item.Owner.PaymentDue += (decimal)item.Owner.Commission * item.Price;
                storeProfit += (1 - (decimal)item.Owner.Commission) * item.Price; //this is fluid for different levels of commission                
            }

            shoppingCartData.Clear();

            // for using a sold property instead of a quantity
            //itemsBinding.DataSource = store.Items.Where(x => x.Sold == false).ToList();

            //play a sound when purchase is made
            try
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"D:\Documents\Visual Studio 2017\Projects\C#\ConsignmentShop\chime.wav");
                player.Play();
            } catch(Exception i)
            {
                Console.WriteLine(i);
            }


            storeProfitValue.Text = string.Format("${0}", storeProfit);                    

            cartBinding.ResetBindings(false);
            itemsBinding.ResetBindings(false);
            vendorsBinding.ResetBindings(false);
        }
    }
}
