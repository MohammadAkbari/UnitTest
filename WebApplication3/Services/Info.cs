using System;

namespace WebApplication3.Services
{
    public class Info
    {
        public Info(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }


    public class TryModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public bool IsClosed { get; set; }
        public int? ParetlD { get; set; }
    }
    public class TryBase
    {
        public static string getData(Func<TryModel, bool> key)
        {
            return "???";
        }
    }

    public class Tryl : TryBase
    {
        public static string doJob(int pld)
        {
            return getData((e)=>e.ParetlD == pld && e.IsClosed == false);
        }
    }
}