using UnityEngine;

public class BotScript: MonoBehaviour {
  public GameObject snake;
  public GameObject waypointsParent;
  public float waypointRadius = 3f;
  public float maxSpeed = 15f;
  public float acceleration = 2.15f;
  public float deceleration = 1.25f;
  public float rotationSpeed = 500f;

  private Transform[] waypoints;
  private int currentWaypointIndex = 0;
  private float currentSpeed = 0f;

  public float brakeDistance = 8f;
  public float turnBrakeDistance = 12f;
  public float turnSlowSpeed = 5f;
  public float straightSpeed = 15f;

  void Start() {

    if (waypointsParent != null) {
      int childCount = waypointsParent.transform.childCount;
      waypoints = new Transform[childCount];

      for (int i = 0; i < childCount; i++) {
        waypoints[i] = waypointsParent.transform.GetChild(i);
      }

      currentWaypointIndex = 0;

      Debug.Log($ "Загружено {waypoints.Length} точек маршрута");
    }
    else {
      Debug.LogError("waypointsParent не назначен! Укажите родительский объект с точками.");
    }
  }

  void Update() {
    if (waypoints == null || waypoints.Length == 0) return;

    Transform targetWaypoint = waypoints[currentWaypointIndex];
    if (targetWaypoint == null) return;

    float distanceToNextWaypoint = Vector3.Distance(snake.transform.position, targetWaypoint.position);

    int nextWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    Transform nextWaypoint = waypoints[nextWaypointIndex];

    int nextNextWaypointIndex = (currentWaypointIndex + 2) % waypoints.Length;
    Transform nextNextWaypoint = waypoints[nextNextWaypointIndex];

    bool isNextWaypointTurn = nextWaypoint.name.ToLower().StartsWith("turn_");

    bool isNextNextWaypointTurn = nextNextWaypoint.name.ToLower().StartsWith("turn_");

    bool isCurrentWaypointTurn = targetWaypoint.name.ToLower().StartsWith("turn_");

    float targetSpeed = maxSpeed;

    if (isNextWaypointTurn) {
      targetSpeed = turnSlowSpeed;
    }

    if (isNextNextWaypointTurn && !isNextWaypointTurn) {
      targetSpeed = Mathf.Lerp(straightSpeed, turnSlowSpeed, 0.5f);
    }

    if (isCurrentWaypointTurn) {
      targetSpeed = turnSlowSpeed;
    }

    float currentBrakeDistance = brakeDistance;

    if (isNextWaypointTurn) {
      currentBrakeDistance = turnBrakeDistance;
    }

    if (distanceToNextWaypoint < currentBrakeDistance) {

      float t = distanceToNextWaypoint / currentBrakeDistance;
      float desiredSpeed = Mathf.Lerp(targetSpeed, currentSpeed, t);

      if (currentSpeed > desiredSpeed) {
        currentSpeed -= Time.deltaTime * deceleration * 2f;
        if (currentSpeed < desiredSpeed) currentSpeed = desiredSpeed;
      }
    }
    else if (currentSpeed < targetSpeed) {

      currentSpeed += Time.deltaTime * acceleration;
      if (currentSpeed > targetSpeed) currentSpeed = targetSpeed;
    }
    else if (currentSpeed > targetSpeed) {

      currentSpeed -= Time.deltaTime * deceleration;
      if (currentSpeed < targetSpeed) currentSpeed = targetSpeed;
    }

    if (currentSpeed < 0.5f) currentSpeed = 0.5f;

    if (currentSpeed > 0) {

      Vector3 directionToWaypoint = (targetWaypoint.position - snake.transform.position).normalized;

      if (directionToWaypoint != Vector3.zero) {
        Quaternion targetRotation = Quaternion.LookRotation(directionToWaypoint);
        snake.transform.rotation = Quaternion.RotateTowards(
        snake.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime * (currentSpeed / maxSpeed));
      }

      snake.transform.position += snake.transform.forward * currentSpeed * Time.deltaTime * 2f;

      if (distanceToNextWaypoint < waypointRadius) {

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

        string pointType = isCurrentWaypointTurn ? "ПОВОРОТ": "прямая";
        Debug.Log($ "Бот достиг точки {currentWaypointIndex} ({pointType})");
      }
    }

    Debug.DrawLine(snake.transform.position, targetWaypoint.position, isCurrentWaypointTurn ? Color.red: Color.green);
    if (nextWaypoint != null) {
      Color nextColor = isNextWaypointTurn ? Color.red: Color.blue;
      Debug.DrawLine(targetWaypoint.position, nextWaypoint.position, nextColor);
    }

    Debug.Log($ "Скорость: {currentSpeed:F1}/{targetSpeed:F1} Дист: {distanceToNextWaypoint:F1}");

    snake.gameObject.GetComponent < Animator > ().speed = currentSpeed / 4;
  }

  void OnDrawGizmosSelected() {
    if (waypointsParent != null) {
      for (int i = 0; i < waypointsParent.transform.childCount; i++) {
        Transform wp = waypointsParent.transform.GetChild(i);
        if (wp != null) {

          if (wp.name.ToLower().StartsWith("turn_")) Gizmos.color = Color.red;
          else Gizmos.color = Color.green;

          Gizmos.DrawWireSphere(wp.position, waypointRadius);
        }
      }
    }
  }
}