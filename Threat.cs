namespace Lab2
{
    public class Threat : TableItem
    {
        public string Description { get; set; }
        public string Origin { get; set; }
        public string Target { get; set; }
        public bool ConfidenceThreat { get; set; }
        public bool IntegrityThreat { get; set; }
        public bool AccessibilityThreat { get; set; }

        public Threat(string id, string name, string description, string origin, string target, bool confidenceThreat, bool integrityThreat, bool accessibilityThreat) : base(id, name)
        {
            Description = description;
            Origin = origin;
            Target = target;
            ConfidenceThreat = confidenceThreat;
            IntegrityThreat = integrityThreat;
            AccessibilityThreat = accessibilityThreat;

        }
        public static bool IsSameThreats(Threat a, Threat b)
        {
            return (a.ID == b.ID) && (a.Name == b.Name) && (a.Description == b.Description) && a.Origin.Equals(b.Origin) && (a.Target == b.Target) && (a.ConfidenceThreat == b.ConfidenceThreat) && (a.IntegrityThreat == b.IntegrityThreat) && (a.AccessibilityThreat == b.AccessibilityThreat);
        }
        public static string FindDifference(Threat a, Threat b)
        {
            if (!(a.ID == b.ID))
            {
                return $"Идентификатор угрозы: БЫЛО {a.ID} - СТАЛО {b.ID}";
            }
            else if (!(a.Name == b.Name))
            {
                return $"Наименование угрозы: БЫЛО {a.Name} - СТАЛО {b.Name}";
            }
            else if (!(a.Description == b.Description))
            {
                return $"Описание угрозы: БЫЛО {a.Description} - СТАЛО {b.Description}";
            }
            else if (!(a.Origin.Equals(b.Origin)))
            {
                return $"Источник угрозы: БЫЛО {a.Origin} - СТАЛО {b.Origin}";
            }
            else if (!(a.Target == b.Target))
            {
                return $"Объект воздействия угрозы: БЫЛО {a.Target} - СТАЛО {b.Target}";
            }
            else if (!a.ConfidenceThreat.Equals(b.ConfidenceThreat))
            {
                return $"Нарушение конфиденциальности: БЫЛО {a.ConfidenceThreat} - СТАЛО {b.ConfidenceThreat}";
            }
            else if (!a.IntegrityThreat.Equals(b.IntegrityThreat))
            {
                return $"Нарушение целостности: БЫЛО {a.IntegrityThreat} - СТАЛО {b.IntegrityThreat}";
            }
            else if (!a.AccessibilityThreat.Equals(b.AccessibilityThreat))
            {
                return $"Нарушение доступности: БЫЛО {a.AccessibilityThreat} - СТАЛО {b.AccessibilityThreat}";
            }
            else
            {
                return null;
            }
        }
    }
}
