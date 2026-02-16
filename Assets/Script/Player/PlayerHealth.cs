using UnityEngine;

public class PlayerHealth : Base, IStart, IUpdate
{
    private Health health = new(0, 100);
    public void OnStart()
    {
        health.Fill();
        switch (health.State)
        {
            case Health.States.Add:
                break;
            case Health.States.Reduce:
                break;
        }
    }

    public void OnUpdate()
    {
        
    }
}