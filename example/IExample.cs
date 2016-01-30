using System;
using System.Collections.Generic;
using Hprose.Common;
using System.Threading.Tasks;

namespace Example {
    public class User {
        public string name;
        public int age;
        public bool male;
        public List<User> friends;
    }

    public interface IExample {
        [SimpleMode(true)]
        Task<string> Hello(string name);
        [ResultMode(HproseResultMode.Serialized)]
        [MethodName("SendUsers")]
        byte[] SendUsersRaw(List<User> users);
        List<User> SendUsers(List<User> users);
        void SendUsers(List<User> users, HproseCallback1<List<User>> callback);
    }
}