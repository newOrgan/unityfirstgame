using UnityEngine;


public class CharacterControllerScript : MonoBehaviour
{
  
    public float maxSpeed = 5f;

    public float jumpSpeed = 2f;

    private bool isFacingRight = true;
    
    private Animator anim;

    private Rigidbody2D rb;

    public LayerMask whatisGround;

    /// <summary>
    /// Начальная инициализация
    /// </summary>
	private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
	
    /// <summary>
    /// Выполняем действия в методе FixedUpdate, т. к. в компоненте Animator персонажа
    /// выставлено значение Animate Physics = true и анимация синхронизируется с расчетами физики
    /// </summary>
	private void FixedUpdate()
    {
        //используем Input.GetAxis для оси Х. метод возвращает значение оси в пределах от -1 до 1.
        //при стандартных настройках проекта 
        //-1 возвращается при нажатии на клавиатуре стрелки влево (или клавиши А),
        //1 возвращается при нажатии на клавиатуре стрелки вправо (или клавиши D)
        float move = Input.GetAxis("Horizontal");
          

        //в компоненте анимаций изменяем значение параметра Speed на значение оси Х.
        //приэтом нам нужен модуль значения
        anim.SetFloat("Speed", Mathf.Abs(move));

        //обращаемся к компоненту персонажа RigidBody2D. задаем ему скорость по оси Х, 
        //равную значению оси Х умноженное на значение макс. скорости

        rb.velocity = new Vector2(move * maxSpeed, rb.velocity.y);
       
        
        //если нажали клавишу для перемещения вправо, а персонаж направлен влево
        if(move > 0 && !isFacingRight)
            //отражаем персонажа вправо
            Flip();
        //обратная ситуация. отражаем персонажа влево
        else if (move < 0 && isFacingRight)
            Flip();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround())
        {
            Jump();
            
        }
    }

    /// <summary>
    /// Метод для смены направления движения персонажа и его зеркального отражения
    /// </summary>
    private void Flip()
    {
        //меняем направление движения персонажа
        isFacingRight = !isFacingRight;
        //получаем размеры персонажа
        Vector3 theScale = transform.localScale;
        //зеркально отражаем персонажа по оси Х
        theScale.x *= -1;
        //задаем новый размер персонажа, равный старому, но зеркально отраженный
        transform.localScale = theScale;
     }

    private void Jump()
    {
        rb.AddForce(new Vector2(0, jumpSpeed),ForceMode2D.Impulse);
    }
    private bool isGround()
    {
        var pos = rb.transform.position;
        return Physics2D.OverlapCircle(pos, 0.3f, whatisGround);
    }
}