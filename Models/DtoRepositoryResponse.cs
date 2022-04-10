namespace ShopAPI.Model
{
    // class передачи сообщений Validation между repository adn controllers
    public class DtoRepositoryResponse 
    {
        public bool Flag { get; set; }
        public string Message { get; set; }
        public object Item { get; set; }
    }


}

