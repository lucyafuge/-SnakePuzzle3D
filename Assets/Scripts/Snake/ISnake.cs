using UnityEngine;

public interface ISnake
{
    /// <summary>
    /// Method for killing a snake section
    /// </summary>
    void Death();
    /// <summary>
    /// Method for moving the snake section into position
    /// </summary>
    /// <param name="position"></param>
    void Move(Vector3 position, float smoothMoveSpeed, Quaternion rotation);
    void Move(Vector3 position, float smoothMoveSpeed, float xRotation);
}
