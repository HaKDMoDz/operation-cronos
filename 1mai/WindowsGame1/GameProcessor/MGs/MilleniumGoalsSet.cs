using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Operation_Cronos.Display;

namespace Operation_Cronos.GameProcessor
{
    public class MilleniumGoalsSet
    {
        #region Fields
        float environment;
        float energy;
        float education;
        float economy;
        float health;
        float food;

        float envQ1 = 0f;
        float envQ2 = 0.1f;
        float eneQ1 = 0f;
        float eneQ2 = 0.1f;
        float eduQ1 = 0.1f;
        float eduQ2 = 0.1f;
        float heaQ1 = 0.3f;
        float heaQ2 = 0.2f;
        float fooQ1 = 0.4f;
        float fooQ2 = 0.3f;
        float ecoQ1 = 0.2f;
        float ecoQ2 = 0.2f;
        #endregion

        #region Public Properties
        public float Environment
        {
            get { return environment; }
            set { environment = value; }
        }

        public float Energy
        {
            get { return energy; }
            set { energy = value; }
        }

        public float Education
        {
            get { return education; }
            set { education = value; }
        }

        public float Economy
        {
            get { return economy; }
            set { economy = value; }
        }

        public float Health
        {
            get { return health; }
            set { health = value; }
        }

        public float Food
        {
            get { return food; }
            set { food = value; }
        }

        public float MeanValueHP1
        {
            get { return (envQ1*environment + eneQ1*energy + ecoQ1*economy + eduQ1*education + fooQ1*food + heaQ1*health) / 6; }
        }

        public float MeanValueHP2
        {
            get { return (envQ2 * environment + eneQ2 * energy + ecoQ2 * economy + eduQ2 * education + fooQ2 * food + heaQ2 * health) / 6; }
        }
        #endregion

        #region Constructors
        public MilleniumGoalsSet()
        {
            environment = 0;
            energy = 0;
            education = 0;
            economy = 0;
            health = 0;
            food = 0;
        }

        public MilleniumGoalsSet(float ec, float health, float edu, float energy, float food, float env)
        {
            economy = ec;
            this.health = health;
            education = edu;
            this.energy = energy;
            this.food = food;
            environment = env;
        }
        #endregion

        #region Operators
        public static MilleniumGoalsSet operator +(MilleniumGoalsSet a, MilleniumGoalsSet b)
        {
            MilleniumGoalsSet c = new MilleniumGoalsSet();
            c.environment = a.environment + b.environment;
            c.energy = a.energy + b.energy;
            c.education = a.education + b.education;
            c.economy = a.economy + b.economy;
            c.health = a.health + b.health;
            c.food = a.food + b.food;
            return c;
        }

        public static MilleniumGoalsSet operator -(MilleniumGoalsSet a, MilleniumGoalsSet b)
        {
            MilleniumGoalsSet c = new MilleniumGoalsSet();
            c.environment = a.environment - b.environment;
            c.energy = a.energy - b.energy;
            c.education = a.education - b.education;
            c.economy = a.economy - b.economy;
            c.health = a.health - b.health;
            c.food = a.food - b.food;
            return c;
        }

        public static MilleniumGoalsSet operator *(MilleniumGoalsSet a, MilleniumGoalsSet b)
        {
            MilleniumGoalsSet c = new MilleniumGoalsSet();
            c.environment = a.environment * b.environment;
            c.energy = a.energy * b.energy;
            c.education = a.education * b.education;
            c.economy = a.economy * b.economy;
            c.health = a.health * b.health;
            c.food = a.food * b.food;
            return c;
        }

        public static MilleniumGoalsSet operator *(MilleniumGoalsSet a, float b)
        {
            MilleniumGoalsSet c = new MilleniumGoalsSet();
            c.environment = a.environment * b;
            c.energy = a.energy * b;
            c.education = a.education * b;
            c.economy = a.economy * b;
            c.health = a.health * b;
            c.food = a.food * b;
            return c;
        }
        public static float operator ^(MilleniumGoalsSet a, MilleniumGoalsSet b)
        {
            float c = 0;
            c = a.environment * b.environment
                + a.energy * b.energy
                + a.education * b.education
                + a.economy * b.economy
                + a.health * b.health
                + a.food * b.food;
            return c;
        }
        #endregion

        public float GetValue(ConstructionType ct)
        {
            float value = 0;

            switch (ct)
            {
                case ConstructionType.Economy:
                    value = Economy;
                    break;
                case ConstructionType.Education:
                    value = Education;
                    break;
                case ConstructionType.Environment:
                    value = Environment;
                    break;
                case ConstructionType.Food:
                    value = Food;
                    break;
                case ConstructionType.Health:
                    value = Health;
                    break;
                case ConstructionType.Energy:
                    value = Energy;
                    break;
            }
            return value;
        }
        public override string ToString()
        {
            return "Ec: " + economy.ToString() + " health: " + health.ToString() + " edu: " + education.ToString() + " energy: " + energy.ToString() + " food: " + food.ToString() + " env: " + environment.ToString();
        }
    }
}
