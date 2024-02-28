/// <summary>
/// TODO: A generic thing for this 
/// </summary>
public class NPCConditionalContext
{
    // e.g. Date night: 
    // IF  
        // (Day 3 Evening OR Day 4 Evening OR Day 5 Evening)
        // AND
        // (Dialogue variable Date_Night_active true)
        // AND 
        // (Mission Custom Nav completed) 
    // THEN 
        // Set animator state - Picnic 
        // Set position - Picnic 
    // ELSE 
        // Set animator state - Idle 
        // Set position - Original 
}
