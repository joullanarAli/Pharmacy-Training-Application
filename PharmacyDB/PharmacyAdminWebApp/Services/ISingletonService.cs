using System;

namespace PharmacyAdminWebApp.Services
{

    public interface ISingletonService
    {
        public int Random { get; set; }

        public int GetRandom();
    }

    public interface IScopedService
    {
        public int Random { get; set; }

        public int GetRandom();
    }

    public interface ITransientService
    {
        public int Random { get; set; }

        public int GetRandom();
    }


    public class AppService : ISingletonService,
        IScopedService, ITransientService
    {

        public AppService() : this(new Random().Next(10))
        {

        }

        public AppService(int rnd)
        {
            this.Random = rnd;
        }

        public int Random { get; set; }

        public int GetRandom()
        {
            return Random;
        }
    }

}
