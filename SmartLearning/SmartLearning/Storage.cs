using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmartLearning
{
    public class Storage
    {
        public static string GetLocalPath(string localFileName)
        {
            string documentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var localPath = Path.Combine(documentPath, localFileName);
            return localPath;
        }



        //internal static Task<T> ReadStorageFile <T>(string fileName)
        //{
        //    Assembly assembly = typeof(SmartLearning.App).GetTypeInfo().Assembly;

        //    var resource = assembly.GetManifestResourceNames();
        //    var filePath = (from f in resource where f.Contains(fileName) select f).FirstOrDefault();

        //    Stream stream = assembly.GetManifestResourceStream(filePath);

        //    using (TextReader reader = new StreamReader(stream))
        //    {

        //        XmlSerializer serializer = new XmlSerializer(typeof(T));
        //        return Task.FromResult<T>(serializer.Deserialize(reader));

        //    }

        //}

        //internal static void SerializeAndWriteList<T>(List<object> data, string questionFile)
        //{
        //    File.WriteAllText(GetLocalPath(questionFile), JsonConvert.SerializeObject(data));
        //}

        internal static void SerializeAndWriteList<T>( List<T> data, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            Stream writer = new FileStream(GetLocalPath(fileName), FileMode.Create);//initialises the writer

            serializer.Serialize(writer, data);//Writes to the file
            writer.Close();
            //File.WriteAllText(GetLocalPath(fileName), serializer.Serialize(GetLocalPath(fileName), data);
        }

        //internal static void SerializeAndWriteList<Object>(List<Object> data, string fileName)
        //{
        //    File.WriteAllText(GetLocalPath(fileName), JsonConvert.SerializeObject(data));
        //}

        //internal static void SerializeAndWriteList <T>(T letters, string fileName)
        //{

        //    File.WriteAllText(GetLocalPath(fileName), JsonConvert.SerializeObject(letters));


        //}

        internal static List<T> ReadListFromDatabase<T>(string fileName)
        {
            Stream reader = new FileStream(GetLocalPath(fileName), FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            var data = serializer.Deserialize(reader) ;

            reader.Close();

            return (List<T>)data;



        }

        // Read storage file
        public static List<T> ReadXMLFromEmbeddedResource<T>(string file)
        {
            try
            {
                //Assembly assembly = typeof(SmartLearning.App).GetTypeInfo().Assembly;
                Assembly assembly = Assembly.GetExecutingAssembly();


                var resource = assembly.GetManifestResourceNames();
                var filePath = (from f in resource where f.Contains(file) select f).FirstOrDefault();

              using(  Stream stream = assembly.GetManifestResourceStream(filePath))

                using (StreamReader sr = new StreamReader(stream))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>));
                    var data= (List<T>)xmlSerializer.Deserialize(sr);
                    return data;

                }
            }
            catch (Exception e)
            {

                Debug.Write(e.GetType());
                return default(List<T>);
            }

        }


    }
}
