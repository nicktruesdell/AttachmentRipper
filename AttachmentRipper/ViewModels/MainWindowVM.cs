using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MsgReader.Outlook;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AttachmentRipper.ViewModels
{
    public class MainWindowVM : ViewModelBase
    {
        #region Fields
        private string _sourceDirectory;
        private string _destDirectory;
        private ICommand _okCommand;
        #endregion

        #region Properties
        public string SourceDirectory
        {
            get => _sourceDirectory;
            set => Set(ref _sourceDirectory, value);
        }

        public string DestDirectory
        {
            get => _destDirectory;
            set => Set(ref _destDirectory, value);
        }

        public ICommand OKCommand => _okCommand ??= new RelayCommand(() =>
        {
			if (string.IsNullOrEmpty(SourceDirectory))
			{
				MessageBox.Show("Source directory must be specified.");
				return;
			}
			if (string.IsNullOrEmpty(DestDirectory))
			{
				MessageBox.Show("Destination directory must be specified.");
				return;
			}
			if (Directory.GetFiles(DestDirectory).Any())
            {
				MessageBox.Show("Destination directory must be empty.");
				return;
            }
			Regex EmailRegex = new Regex("^.*msg$");
			Regex ExcelRegex = new Regex("^.*xlsx$");
			foreach (string MessagePath in Directory.GetFiles(SourceDirectory).Where(x => EmailRegex.IsMatch(x)))
			{
				var Message = new Storage.Message(MessagePath);
				foreach (var Attachment in Message.Attachments)
				{
					if (Attachment is Storage.Attachment)
					{
						var TypedAttachment = Attachment as Storage.Attachment;
						if (ExcelRegex.IsMatch(TypedAttachment.FileName))
						{
							using (FileStream fs = File.Create(GetUniqueFileName(TypedAttachment.FileName)))
							{
								fs.Write(TypedAttachment.Data, 0, TypedAttachment.Data.Length);
							}
						}
					}
				}
			}
			MessageBox.Show("Finished");
			return;
		});

		private string GetUniqueFileName(string FileName)
        {
			string StartingName = string.Format(@"{0}\{1}", DestDirectory, FileName);
			if (!File.Exists(string.Format(StartingName)))
			{
				return StartingName;
			}
			else
            {
				int counter = 1;
				string OnlyName = Path.GetFileNameWithoutExtension(StartingName);
				string Extension = Path.GetExtension(StartingName);
				string NameWithCount = string.Format(@"{0}\{1} ({2}{3}", DestDirectory, OnlyName, counter, Extension);
				while (File.Exists(NameWithCount))
                {
					counter++;
                }
				return NameWithCount;
            }
        }
        #endregion
    }
}
