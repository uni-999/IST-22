using UnityEngine;

public class BotScript : MonoBehaviour
{
    public GameObject snake;
    public GameObject waypointsParent; // родительский объект, содержащий все точки как дочерние
    public float waypointRadius = 3f; // радиус, при котором точка считается пройденной
    public float maxSpeed = 15f;
    public float acceleration = 2.15f;
    public float deceleration = 1.25f;
    public float rotationSpeed = 500f;
    public float boost = 1f;

    private Transform[] waypoints; // массив точек (дочерних объектов)
    private int currentWaypointIndex = 0;
    public float currentSpeed = 0f;

    // Параметры для торможения перед поворотами
    public float brakeDistance = 8f; // расстояние, на котором начинаем тормозить (увеличил)
    public float turnBrakeDistance = 5f; // расстояние для торможения перед поворотом
    public float turnSlowSpeed = 5f; // скорость прохождения поворотов
    public float straightSpeed = 15f; // скорость на прямых

    public GameObject trapPrefab;
    public int countLaps;

    void Start()
    {
        // Получаем все дочерние объекты в порядке их иерархии
        if (waypointsParent != null)
        {
            countLaps = 0;
            int childCount = waypointsParent.transform.childCount;
            waypoints = new Transform[childCount];

            for (int i = 0; i < childCount; i++)
            {
                waypoints[i] = waypointsParent.transform.GetChild(i);
            }

            // Начинаем с первой точки (индекс 0)
            currentWaypointIndex = 0;

            //Debug.Log($"Загружено {waypoints.Length} точек маршрута");
        }
        else
        {
            Debug.LogError("waypointsParent не назначен! Укажите родительский объект с точками.");
        }
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        // Получаем текущую целевую точку
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        if (targetWaypoint == null) return;

        // Расстояние до текущей точки
        float distanceToNextWaypoint = Vector3.Distance(snake.transform.position, targetWaypoint.position);

        // Получаем следующую точку
        int nextWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        Transform nextWaypoint = waypoints[nextWaypointIndex];

        // Получаем точку через одну (для более плавного торможения)
        int nextNextWaypointIndex = (currentWaypointIndex + 2) % waypoints.Length;
        Transform nextNextWaypoint = waypoints[nextNextWaypointIndex];

        // --- АНАЛИЗ ПОВОРОТОВ ПО НАЗВАНИЯМ ТОЧЕК ---

        // Проверяем, является ли следующая точка поворотом
        bool isNextWaypointTurn = nextWaypoint.name.ToLower().StartsWith("turn_");

        // Проверяем, является ли текущая точка поворотом (для выхода из поворота)
        bool isCurrentWaypointTurn = targetWaypoint.name.ToLower().StartsWith("turn_");

        // --- ЛОГИКА ОПРЕДЕЛЕНИЯ ЦЕЛЕВОЙ СКОРОСТИ ---

        float targetSpeed = maxSpeed; // целевая скорость по умолчанию

        // Если следующая точка - поворот, готовимся тормозить
        if (isNextWaypointTurn)
        {
            targetSpeed = turnSlowSpeed;
        }

        // Если мы сейчас в повороте, то поддерживаем низкую скорость
        if (isCurrentWaypointTurn)
        {
            targetSpeed = turnSlowSpeed;
        }

        // --- ЛОГИКА УСКОРЕНИЯ И ТОРМОЖЕНИЯ С УЧЕТОМ ПОВОРОТОВ ---

        // Определяем расстояние для торможения в зависимости от ситуации
        float currentBrakeDistance = brakeDistance;

        // Если впереди поворот, увеличиваем дистанцию торможения
        if (isNextWaypointTurn)
        {
            currentBrakeDistance = turnBrakeDistance;
        }

        // Управление скоростью в зависимости от расстояния до точки
        if (distanceToNextWaypoint < currentBrakeDistance)
        {
            // Плавно тормозим до целевой скорости
            float t = distanceToNextWaypoint / currentBrakeDistance; // 0 у точки, 1 на границе торможения
            float desiredSpeed = Mathf.Lerp(targetSpeed, currentSpeed, t);

            if (currentSpeed > desiredSpeed)
            {
                currentSpeed -= Time.deltaTime * deceleration * 2f; // тормозим быстрее
                if (currentSpeed < desiredSpeed) currentSpeed = desiredSpeed;
            }
        }
        else if (currentSpeed < targetSpeed)
        {
            // Разгоняемся до целевой скорости
            currentSpeed += Time.deltaTime * acceleration;
            if (currentSpeed > targetSpeed) currentSpeed = targetSpeed;
        }
        else if (currentSpeed > targetSpeed)
        {
            // Если скорость выше целевой, плавно снижаем
            currentSpeed -= Time.deltaTime * deceleration;
            if (currentSpeed < targetSpeed) currentSpeed = targetSpeed;
        }

        // Минимальная скорость
        if (currentSpeed < 0.5f) currentSpeed = 0.5f;

        // --- ДВИЖЕНИЕ К ТОЧКЕ ---
        if (currentSpeed > 0)
        {
            // Направление к текущей точке
            Vector3 directionToWaypoint = (targetWaypoint.position - snake.transform.position).normalized;

            // Плавный поворот к точке
            if (directionToWaypoint != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToWaypoint);
                snake.transform.rotation = Quaternion.RotateTowards(
                    snake.transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime * (currentSpeed / maxSpeed)
                );
            }

            // Движение вперед с текущей скоростью
            snake.transform.position += snake.transform.forward * currentSpeed * Time.deltaTime * boost;

            // Проверка достижения точки
            if (distanceToNextWaypoint < waypointRadius)
            {
                // Переходим к следующей точке
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

                // Логируем информацию о точке
                string pointType = isCurrentWaypointTurn ? "ПОВОРОТ" : "прямая";
                //Debug.Log($"Бот достиг точки {currentWaypointIndex} ({pointType})");
            }
        }

        // Визуализация для отладки
        Debug.DrawLine(snake.transform.position, targetWaypoint.position, isCurrentWaypointTurn ? Color.red : Color.green);
        if (nextWaypoint != null)
        {
            Color nextColor = isNextWaypointTurn ? Color.red : Color.blue;
            Debug.DrawLine(targetWaypoint.position, nextWaypoint.position, nextColor);
        }

        // Отображаем текущую скорость и целевую скорость
        //Debug.Log($"Скорость: {currentSpeed:F1}/{targetSpeed:F1} Дист: {distanceToNextWaypoint:F1}");

        snake.gameObject.GetComponent<Animator>().speed = currentSpeed / 4;
    }

    // Визуализация радиуса точек в редакторе
    void OnDrawGizmosSelected()
    {
        if (waypointsParent != null)
        {
            for (int i = 0; i < waypointsParent.transform.childCount; i++)
            {
                Transform wp = waypointsParent.transform.GetChild(i);
                if (wp != null)
                {
                    // Разным цветом отмечаем повороты и прямые
                    if (wp.name.ToLower().StartsWith("turn_"))
                        Gizmos.color = Color.red;
                    else
                        Gizmos.color = Color.green;

                    Gizmos.DrawWireSphere(wp.position, waypointRadius);
                }
            }
        }
    }
}