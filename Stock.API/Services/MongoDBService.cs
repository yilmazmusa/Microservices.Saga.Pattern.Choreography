using MongoDB.Driver;

namespace Stock.API.Services
{
    public class MongoDBService
    {
        readonly IMongoDatabase _mongoDatabase;

        public MongoDBService(IConfiguration configuration) // Bu configuration üzerinden MongoDB ile bağlantı kuracağız.Dependency Injection la alakalı mesela bunun tipini string verseydik DP den fydalanamayacaktık.
        {
            MongoClient client = new(configuration.GetConnectionString("MongoDB")); //MongoDB databesi'imize eriştik.
            _mongoDatabase = client.GetDatabase("StockDB"); //MongoDB database'imizin adını verdik ve client üzerinden eriştik.
                                                            //Artık MongoDB'ye _mongoDatabase diyerek erişebiliriz.
        }


        //Artık MongoDB içerisindeki dataları/collectionları buraya çekmemizi sağlayacak fonk yazacağız.
        public IMongoCollection<T> GetCollection<T>() => _mongoDatabase.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
        //Şimdi yukarda diyoruzki;
        //Ben MongoDB database'ime gidip GetCollection metodu ile  ordan data/collection çekicem diyorum.
        //Ama Benim MongoDB database'imde birden fazla türde Ürün/Product olabilir.O yüzden  typeof(T).Name.ToLowerInvariant() diyoruz yani name i küçült ve name inden yakala
        //O yüzden çekeceğim ürünün tipi ne ise generic onu karşıla diyorum T ile
        //Yani MongoDB database'den Elma çektim tipi Elma olacak, Armut çektim Armut olucak.
    }
}
