using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WeHaveSecrets.Models;
using WeHaveSecrets.Repositories;

namespace WeHaveSecrets.Services
{
    public class DatabaseMaintenance : IDatabaseMaintenance
    {
        private readonly string _baseUrl;
        private readonly string _backupFolder;
        private readonly IMaintenanceRepository _maintenanceRepository;

        public DatabaseMaintenance(string baseUrl, string backupFolder, IMaintenanceRepository maintenanceRepository)
        {
            if (baseUrl == null) throw new ArgumentNullException("baseUrl");
            if (backupFolder == null) throw new ArgumentNullException("backupFolder");
            if (maintenanceRepository == null) throw new ArgumentNullException("maintenanceRepository");

            _baseUrl = baseUrl;
            _backupFolder = backupFolder;
            _maintenanceRepository = maintenanceRepository;
        }

        public bool Backup()
        {
            EnsureBackupFolderExists();

            var dateTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            var filename = $"{_backupFolder}/WeHaveSecrets.{dateTime}.bak";
            try
            {
                return _maintenanceRepository.BackupTo(filename);
            }
            catch (Exception ex)
            {
                // Log and raise as issue
            }

            return false;
        }

        public List<Backup> Backups()
        {
            EnsureBackupFolderExists();

            return Directory.GetFiles(_backupFolder, "*.bak")
                            .Select(filepath => new Backup
                            {
                                Url = ConvertToUrl(filepath),
                                Created = File.GetCreationTimeUtc(filepath)
                            })
                            .ToList();
        }

        private void EnsureBackupFolderExists()
        {
            if (!Directory.Exists(_backupFolder))
            {
                Directory.CreateDirectory(_backupFolder);
            }
        }

        public string ConvertToUrl(string filepath)
        {
            var filename = Path.GetFileName(filepath);

            return $"{_baseUrl}\\{filename}";
        }
    }
}
