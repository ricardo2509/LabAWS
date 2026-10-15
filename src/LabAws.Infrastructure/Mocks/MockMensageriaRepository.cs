using LabAws.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabAws.Infrastructure.Mocks
{
    public class MockMensageriaRepository : IMensageriaRepository
    {
        public async Task<string> SendNotificationAsync(string fileName, string bucketName)
        {
            return await Task.FromResult($"Message sent to {bucketName} queue");
        }
    }
}