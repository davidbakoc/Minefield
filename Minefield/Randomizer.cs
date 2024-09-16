using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minefield
{
	public interface IRandomizer
	{
		int Next();
	}

	public class Randomizer : IRandomizer
	{
		private readonly Random random = new Random();

		public int Next() =>
			random.Next();
	}
}
