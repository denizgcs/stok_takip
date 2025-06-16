using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stok_takip.assets
{
    public class TedarikciModel
    {
        
        private int id;
        private string ad;
        private string contactPerson;
        private string phone;
        private string email;
        private string address;

        
        public int Id { get => id; set => id = value; }
        public string Ad { get => ad; set => ad = value; }
        public string ContactPerson { get => contactPerson; set => contactPerson = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Email { get => email; set => email = value; }
        public string Address { get => address; set => address = value; }

        public TedarikciModel() { } 

    }
}