using System.Collections.Generic;
using System.Linq;

namespace RunningClubApp
{
    /// <summary>
    /// Менеджер, определяющий текущее состояние отображения (список бегунов и набор полей).
    /// </summary>
    public class RunnerMngr
    {
        // Списки, для чтения доступные всем, но для изменений открытые только внутри класса-менеджера
        public List<Runner> Runners { get; private set; } 
        public List<RunnerField> DisplayFields { get; private set; }

        public RunnerMngr(List<Runner> runners = null)
        {
            Runners = runners ?? new List<Runner>();
            // По умолчанию показывается 4 поля
            DisplayFields = new List<RunnerField> { new NameField(), new CityField(), new PhoneField(), new GroupField() };
        }

        /// <summary>
        /// Замена списка на новый после фильтрации
        /// </summary>
        /// <returns> Отфильтрованный список </returns>
        public void SetRunners(List<Runner> runners)
        {
            Runners = runners;
        }

        /// <summary>
        /// Замена полей после фильтрации
        /// </summary>
        /// <returns> Отфильтрованные поля </returns>
        public void SetFields(List<RunnerField> fields)
        {
            DisplayFields = fields;
        }
    }
}