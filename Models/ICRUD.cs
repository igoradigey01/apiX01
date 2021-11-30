using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ShopAPI.Model
{



    // не реализован  -temp --Create, Read, Update & Delete 
    public interface ICRUD
    {
        public object[] Get(int id);
        public object Item(int id);
        public bool Create(object item);
        public bool Update(object item);
        public bool Delete(object item);


    }


}

