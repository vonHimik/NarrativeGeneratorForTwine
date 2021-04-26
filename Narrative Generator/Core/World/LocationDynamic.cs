using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Narrative_Generator
{
    /// <summary>
    /// Класс, реализующий динамическую (часто изменяюмую) часть локации.
    /// </summary>
    [Serializable]
    public class LocationDynamic : ICloneable
    {
        // Ссылка на статическую часть локации.
        private LocationStatic locationInfo;

        // Список агентов в локации.
        private Dictionary<AgentStateStatic, AgentStateDynamic> agentsAtLocations;

        // Флаг обозначающий, содержит локация улику или же нет.
        private bool containEvidence;

        // Числовой идентефикатор локации.
        int id;

        /// <summary>
        /// Метод-конструктор для динамической части локации, без параметров.
        /// </summary>
        public LocationDynamic()
        {
            locationInfo = new LocationStatic();
            agentsAtLocations = new Dictionary<AgentStateStatic, AgentStateDynamic>();
            containEvidence = false;

            // Присваиваем случайное целочисленное ID.
            Random rand = new Random();
            id = rand.Next(1000);
        }

        /// <summary>
        /// Метод-конструктор для динамической части локации, в качестве параметра используется значение флага о наличии улики.
        /// </summary>
        /// <param name="containEvidence"></param>
        public LocationDynamic(bool containEvidence)
        {
            locationInfo = new LocationStatic();
            agentsAtLocations = new Dictionary<AgentStateStatic, AgentStateDynamic>();
            this.containEvidence = containEvidence;
        }

        /// <summary>
        /// Метод-конструктор для динамической части локации, 
        /// в качестве параметра использующий значения для флага о наличии улики и ссылку на статическую часть локации.
        /// </summary>
        /// <param name="containEvidence"></param>
        /// <param name="locationInfo"></param>
        public LocationDynamic(bool containEvidence, LocationStatic locationInfo)
        {
            this.locationInfo = locationInfo;
            agentsAtLocations = new Dictionary<AgentStateStatic, AgentStateDynamic>();
            this.containEvidence = containEvidence;
        }

        /// <summary>
        /// Возвращает клон вызвавшей данный метод динамической части локации.
        /// </summary>
        public object Clone()
        {
            // Создаём пустую инстанцию динамической части локации.
            var clone = new LocationDynamic();

            // Проходимся по каждому агенту из списка находящихся в локации, отдельно клонируем их статическую и динамическую части, 
            //   а затем передаём их клону (собрав в одно целое).
            foreach (var agent in this.agentsAtLocations)
            {
                AgentStateStatic sTemp = (AgentStateStatic)agent.Key.Clone();
                AgentStateDynamic dTemp = (AgentStateDynamic)agent.Value.Clone();
                clone.agentsAtLocations.Add(sTemp, dTemp);

                // Очистка
                sTemp = null;
                dTemp = null;
                GC.Collect();
            }

            // Копируем значение флага.
            clone.containEvidence = containEvidence;

            // Возвращаем клона.
            return clone;
        } 

        /// <summary>
        /// Добавляет агента в список агентов находящихся в данной локации.
        /// </summary>
        /// <param name="agent"></param>
        public void AddAgent(KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            foreach (var a in agentsAtLocations)
            {
                if (a.Key.GetName().Equals(agent.Key.GetName()))
                {
                    return;
                }
            }

            AgentStateStatic sPrefab = (AgentStateStatic)agent.Key.Clone();
            AgentStateDynamic dPrefab = (AgentStateDynamic)agent.Value.Clone();
            agentsAtLocations.Add(sPrefab, dPrefab);

            // Очистка
            sPrefab = null;
            dPrefab = null;
            GC.Collect();
        }

        /// <summary>
        /// Добавляет набор агентов к списку агентов находящихся в данной локации.
        /// </summary>
        /// <param name="agents"></param>
        public void AddAgents(Dictionary<AgentStateStatic, AgentStateDynamic> agents)
        {
            agentsAtLocations = agentsAtLocations.Concat(agents).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Возвращает указанного агента из списка агентов находящихся в локации.
        /// </summary>
        /// <param name="agent"></param>
        public KeyValuePair<AgentStateStatic, AgentStateDynamic> GetAgent(KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            // Проходимся по всем агентам в локации.
            foreach (var a in agentsAtLocations)
            {
                // Сравниваем имя проверяемого агента с именем искомого агента.
                if (a.Key.GetName() == agent.Key.GetName())
                {
                    // Возвращаем агента при совпадении.
                    return a;
                }
            }

            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Возвращает список агентов находящихся в данной локации.
        /// </summary>
        public Dictionary<AgentStateStatic, AgentStateDynamic> GetAgents()
        {
            // Создаём новый пустой словарь.
            Dictionary<AgentStateStatic, AgentStateDynamic> agents = new Dictionary<AgentStateStatic, AgentStateDynamic>();

            // Проходимся по списку агентов находящихся в данной локации и добавляем их в свежесозданный словарь.
            foreach (var agent in agentsAtLocations)
            {
                agents.Add(agent.Key, agent.Value);
            }

            // Возвращаем этот словарь.
            return agents;
        }

        /// <summary>
        /// Удаляет указанного агента из списка агентов в данной локации, возвращая true при успехе, и false при неудаче.
        /// </summary>
        /// <param name="agent"></param>
        public bool RemoveAgent(KeyValuePair<AgentStateStatic, AgentStateDynamic> agent)
        {
            // Проходимся по всем агентам из списка агентов находящихся в данной локации.
            foreach (var a in agentsAtLocations)
            {
                // Сравниваем имя агента с именем искомого агента.
                if (a.Key.GetName() == agent.Key.GetName())
                {
                    // При совпадении удаляем данного агента и возвращаем true.
                    if (agentsAtLocations.Remove(a.Key)) { return true; }
                    // Если не можем найти такого агента в списке, то возвращаем false.
                    else { return false; }
                }
            }

            return false;
        }

        /// <summary>
        /// Очищает список агентов находящихся в локации.
        /// </summary>
        public void ClearLocation()
        {
            foreach (var agent in agentsAtLocations.ToArray())
            {
                agentsAtLocations.Remove(agent.Key);
            }
        }

        /// <summary>
        /// Ищет указанного агента в списке агентов находящихся в данной локации, при успехе возвращает true, при неудаче возвращает false.
        /// </summary>
        /// <param name="agent"></param>
        public bool SearchAgent(AgentStateStatic agent)
        {
            // Проходимся по всем агентам в списке агентов находящихся в данной локации.
            foreach (var a in agentsAtLocations)
            {
                // Сравниваем имя агента с именем искомого агента.
                if (a.Key.GetName().Equals(agent.GetName()))
                {
                    // При успехе поиска возвращаем true.
                    return true;
                }
            }

            // Иначе возвращаем false.
            return false;
        }

        /// <summary>
        /// Возвращает количество агентов, находящихся в данной локации.
        /// </summary>
        public int CountAgents()
        {
            return agentsAtLocations.Count();
        }

        /// <summary>
        /// Возвращает значение того, есть ли в данной локации улика или нет.
        /// </summary>
        public bool CheckEvidence()
        {
            return containEvidence;
        }

        /// <summary>
        /// Устанавливает ссылку на указанную статическую часть локации.
        /// </summary>
        /// <param name="locationInfo"></param>
        public void SetLocationInfo(LocationStatic locationInfo)
        {
            this.locationInfo = locationInfo;
        }

        /// <summary>
        /// Возвращает статическую часть данной локации.
        /// </summary>
        public LocationStatic GetLocationInfo()
        {
            return locationInfo;
        }
    }
}
