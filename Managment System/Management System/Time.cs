using System;


namespace Management_System
{
    /**
    * Klasa tworząca obiekty zwracające timestampy
    * @Time
    */
    class Time
    {

        /**
        * Metoda zwracająca timestamp
        * @param DateTime dt
        */
        public string GetTimestamp(DateTime dt)
        {
            return dt.ToString("[yyyy-MM-dd HH:mm:ss]");
        }

    }
}
