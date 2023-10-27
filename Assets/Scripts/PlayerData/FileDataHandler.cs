using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class FileDataHandler<T>
{
    string dataPath;
    string fileName;
	bool useEncryption;
	bool logErrors;
	string encryptionKey = "Why bother dude";
	public FileDataHandler(string dataPath, string fileName, bool useEncryption = false,bool logErrors = false)
	{
		this.dataPath = dataPath;
		this.fileName = fileName;
		this.logErrors = logErrors;
		Debug.Log(Path.Combine(dataPath, fileName));
		this.useEncryption = useEncryption;

	}

	public T Load()
	{
		string path = Path.Combine(dataPath, fileName);

		T data = default(T);
		try
		{
			Directory.CreateDirectory(Path.GetDirectoryName(path));

			string storedData = "";

			using (FileStream stream = new FileStream(path, FileMode.Open))
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					storedData = reader.ReadToEnd();
				}
			}

			if (useEncryption)
			{
				storedData = EncryptDecrypt(storedData);
			}

			data = JsonUtility.FromJson<T>(storedData);
			
		}
		catch (Exception ex)
		{
			if(logErrors)
			Debug.LogError("Error when saving data to file: " + path + "\n" + ex);
		}
		return data;
	}

	public void Save(T data)
	{
		string path = Path.Combine(dataPath, fileName);
		try
		{
			Directory.CreateDirectory(Path.GetDirectoryName(path));

			string storedData = JsonUtility.ToJson(data,true);

			if (useEncryption)
			{
				storedData = EncryptDecrypt(storedData);
			}
			using(FileStream stream = new FileStream(path, FileMode.Create))
			{
				using(StreamWriter writer = new StreamWriter(stream))
				{
					writer.Write(storedData);
				}
			}
		}
		catch (Exception ex)
		{
            if (logErrors)
                Debug.LogError("Error when saving data to file: " + path + "\n" + ex);
		}
	}

	
	public FileInfo[] Test(string path)
	{
		DirectoryInfo info = Directory.CreateDirectory(Path.GetDirectoryName(path));
		Debug.Log(info.FullName);
		return info.GetFiles();
	}
	string EncryptDecrypt(string data)
	{
		string modifiedData = "";
		for(int i = 0; i < data.Length; i++)
		{
			modifiedData += (char) (data[i] ^ encryptionKey[i % encryptionKey.Length]);
		}
		return modifiedData;

	}
}
