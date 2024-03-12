using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iot.Greenhouse.Nodes;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Iot.Greenhouse.Data
{
    public class GreenhouseDataSeeder : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Node, int> _nodeRepository;

        public GreenhouseDataSeeder(IRepository<Node, int> nodeRepository)
        {
            _nodeRepository = nodeRepository;
        }
        public async Task SeedAsync(DataSeedContext context)
        {
            if (await _nodeRepository.GetCountAsync() > 0)
                return;
            for (int i = 0; i < 3; i++)
            {
                await _nodeRepository.InsertAsync(new Node()
                {
                    NodeName = $"Node {i+1}",
                });
            }
        }
    }
}
