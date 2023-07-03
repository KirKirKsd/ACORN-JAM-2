using UnityEngine;

public class PlayerMotor : MonoBehaviour {

	// input
	private Controls actions;
	private Controls.PlayerActions playerActions;

	[Header("Movement")]
	[SerializeField] private float speed;

	[Header("Jump")]
	[SerializeField] private float jumpForce;
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private Transform feet;

	[Header("Weapons")]
	[SerializeField] private Transform hadnsPivot;
	private int currentGun;
	[SerializeField] private GameObject[] Weapons;

	// components
	private Rigidbody2D rb;

	private void Awake() {
		// задаю значения
		actions = new Controls();
		playerActions = actions.Player;

		// регистрирую функцию что бы она выполнялась при нажатии на кнопку
		playerActions.Jump.performed += _ => Jump();
		playerActions.Shoot.performed += _ => Shoot();
	}

	private void OnEnable() {
		// при включении персонажа включается и схема управления
		playerActions.Enable();
	}

	private void Start() {
	   // получаю компоненты
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update() {
		print(playerActions.ChangeWeapon.ReadValue<float>());
		Movement();
	}

	// движение персонажа
	private void Movement() {
		var input = playerActions.Movement.ReadValue<float>(); // получаю значение управления оси X
		var velocity = new Vector2(input * speed, rb.velocity.y); // задаю :Vector2 (x = значение управления X * скорость, y = сила физики по оси Y)
		rb.velocity = velocity; // задаю физике значения
	}

	// прыжок персонажа
	private void Jump() {
		if (!IsGrounded()) return; // если персонаж находится не на земле то функция не выполняется

		var velocity = new Vector2(rb.velocity.x, jumpForce); // задаю :Vector2 (x = сила физики по оси X, y = сила прыжка)
		rb.velocity = velocity; // задаю физике значения
	}

	// проверка находится ли персонаж на земле
	private bool IsGrounded() {
		return Physics2D.OverlapCircle(feet.position, 0.05f, groundLayer); // проверяем и возвращаем :bool = находится ли в области ног земля
	}

	// стрельба
	private void Shoot() {
		// смотрим какое оружие выбрано и вызываем у нужного скрипта функцию Shoot()
		switch (currentGun) {
			case 0:
				// GetComponentInChildren<LazerGun>().Shoot();
				break;
			case 1:
				// GetComponentInChildren<Pistol>().Shoot();
				break; 
			case 2:
				// GetComponentInChildren<Hands>().Shoot();
				break;
		}
	}

	// выбор оружия (в скобках :int который указывает на оружие котрое надо сменить)
	private void ChangeWeapon(int changeTo) {
		// на случай если выйдет за рамки массива
		switch (changeTo) {
			case -1:
				changeTo = 2;
				break;
			case 3:
				changeTo = 0;
				break;
		}
		Weapons[currentGun].SetActive(false); // выключаем прошлое оружие
		Weapons[changeTo].SetActive(true); // включаем оружие которое выбрал игрое

		currentGun = changeTo; // теперь переменная выбранного оружия соответствует выбранному оружию
	}

	private void OnDisable() {
		// при выключении персонажа выключается и схема управления
		playerActions.Disable();
	}

}