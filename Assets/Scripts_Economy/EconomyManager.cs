using SimAirport.AI;
using UnityEngine;

namespace SimAirport.Economy {
	/// <summary>
	/// Manages player finances and tracks key economic stats.
	/// </summary>
	public class EconomyManager : MonoBehaviour {
		public int CurrentCash { get; private set; } = 50000; // Starting cash
		public int PassengersServed { get; private set; } = 0;

		private const int FEE_PER_PASSENGER = 100;

		public bool CanAfford(int cost) => CurrentCash >= cost;

		public void Spend(int amount) {
			CurrentCash -= amount;
			// TODO: Raise OnCashChanged event
		}

		public void AddRevenue(int amount) {
			CurrentCash += amount;
			// TODO: Raise OnCashChanged event
		}

		public void OnPassengerBoarded(PassengerAgent passenger) {
			PassengersServed++;
			AddRevenue(FEE_PER_PASSENGER);
			// TODO: Raise OnPassengersServedChanged event
		}
	}
}