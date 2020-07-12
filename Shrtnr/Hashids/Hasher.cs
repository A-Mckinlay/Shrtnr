using System;
using HashidsNet;

namespace Shrtnr.HashIds
{
    public class Hasher : IHasher
    {
        private string _salt;

        public Hasher(string salt)
        {
            _salt = salt;
        }

        public string GetCode()
        {
            var hashids = new Hashids(_salt, 6);
            int timestamp = Math.Abs((int)(Int64)(DateTime.UtcNow.Subtract(DateTime.UnixEpoch)).TotalMilliseconds);
            return hashids.Encode(timestamp);
        }
    }
}
