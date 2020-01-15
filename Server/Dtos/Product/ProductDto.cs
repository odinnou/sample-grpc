namespace Server.Dtos.Product
{
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    public class ProductDto
    {
        public string Reference { get; set; }
        public int Order { get; set; }

        /// <summary>
        /// Produit issue de l'agriculture biologique
        /// </summary>
        public bool IsBio { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
}
