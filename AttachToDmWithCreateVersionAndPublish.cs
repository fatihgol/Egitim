//Kullamılacak referans dosya
%SystemPath%\Common\eBAPI.dll
 
//Kullanılacak Namespaceler
using eBAPI;
using eBAPI.Connection;
using eBAPI.DocumentManagement;
 
 
eBAConnection con = CreateServerConnection();
try{
	con.Open();
	FileSystem fs = con.FileSystem;
	int formId = 1;
	DMFile form = fs.GetFile("workflow/Proje/Form/"+formId+".wfd");
	string categoryName = "default";
	string targetFolder = "Kütüphane/Test/"+DateTime.Now.Year.ToString()+"/"+DateTime.Now.Month.ToString("D2");
	foreach(DMFileContent dmc in form.GetAttachments(categoryName ))
	{ 
		string attName = dmc.ContentName;
		if(fs.HasFolder(targetFolder + "/" + categoryName))
	    	fs.CreateFolder(targetFolder + "/" + categoryName);
	        
		string fileFullPath = targetFolder + "/" + attName;
		if(!fs.HasFile(fileFullPath ))
		{
			DMFile att = fs.CreateFile(fileFullPath);
			att.UploadContentFromByteArray(form.DownloadAttachmentContentBytes(categoryName,attName )); 
		}
		else
		{
			DMFile versionFile = fs.CreateFileMajorVersion(fileFullPath);
            versionFile.UploadContentFromByteArray(form.DownloadAttachmentContentBytes(categoryName,attName));
            
			//Yayınlama isteniliyorsa
			//versionFile.SetPublishedVersion(versionFile.Version);
		}
	}
}finally{
	con.Close();
}
