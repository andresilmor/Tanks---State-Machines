using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankFuel : MonoBehaviour
{
    [Header("Tank Fuel Config:")]
    [SerializeField] float fuel = 50f;
    [SerializeField] float maxFuel = 50f;
    [SerializeField] int consumePerTime = 1;
    [SerializeField] float secondsPerConsume = 60f;
    [SerializeField] float minRefillSeconds = 10f;
    [SerializeField] float maxRefillSeconds = 20f;

    NavMeshAgent navMeshAgent;
    private bool depositEmpty = false;
    private float timer = 0;
    private bool isRefillingDeposit = false;

    private float refillQuantity;

    //Nota: O dano foi diminuido 

    //(Como este script foi feito de raiz por mim vou justificar algumas coisas já aqui em vez de espalhar)

    //> Como não percebi se os 10-20 segundos era para, por exemplo o deposito todo, o deposito descontando a autonomia minima, ou a diferença do total com a quantidade no momento em que começa a encher o deposito, o que fiz foi com a ultima opção. 
    //Para isso, neste caso o RefillFuelDecision irá ver se o tank ja se encontra a encher o deposito, caso nao ele irá chamar o método UpdateRefillQuantity que irá calcular a diferença entre o fuel do momento e o maximo, essa diferença, seja qual for, demorará entre 10-20 segundos para voltar ao normal.
    //Escolhi essa forma porque o "deposito todo" ia dar problemas com as percentagens e usando as percentagens, caso a percentagem da vida fosse alta, tipo 90%(90 de vida) e o tank continua-se a ser atacado e só começa-se a reparar já com 5 de vida, iria demorar imenso a terminar
    //Este método também é aplicado em relação à Vida, no caso das Balas não foi necessário.

    //> O motivo de definir essas informações de percentagens na Decision foi por fazer mais sentido para mim, visto que essa informação existe apenas por causa da "Decision", e só por "consequência" é que afeta o tank.

    //> A ação relacionada com o movimentar para os diversos waypoints (MovementAction) resume-se a um script reaproveitado para os três casos, em cada caso eu dou o prefab do local, por exemplo a AmmoStorageBase, e procuro no cenário por instancias desse mesmo prefab, procurando especificamente pela Tag, dito isto o objetivo era poder implementar um metodo que escolhesse o local mais proximo no momento mas quando crio novas intancias os tanks iam como desejado mas ficavam presos e nao acontecia nada, enquanto que o Player atravessava essa instancias mesmo havendo colliders e rigidbodys, por isso deve ser algo que nao sei e desisiti dessa parte

    //> O motivo de usar esse metodo de passar gameobjects para comparo de tags tanto no MovementAction como em outros como RepairAction serve para facilitar o processo caso necessário alterações nas tags ou no waypoint pretendido sem ser necessário mexer no código, assim como minimizar o uso de "strings em bruto" como ids

    //> Por fim, o state ArrivedDecision, responsavel pela parte Chegou/Não Chegou foi criado apenas uma vez e reaproveitado visto tratar-se de algo simples e repetivel, o mesmo aplica a outras decision como RefillFuelDecision que encontra-se de forma a poder ser usado tanto no momento de decisao enquanto o PatrolChaser está ativo, como no proprio state RefillChaser, desta forma o codigo é mais otimizado e de fácil alteração. Tambem foi dessa forma que interpretei o esquema, em todos os testes que fiz nao houve problemas.

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        Debug.Log("//--------------------------------//");
        Debug.Log("  The Red one is a runaway coward."); // Tank AI Scanner
        Debug.Log("  The Blue one is a crazy berserk."); // Tank AI Chaser
        Debug.Log("//--------------------------------//");
    }

    void Update()
    {
        
        if(navMeshAgent && !navMeshAgent.isStopped)
            ConsumeFuel();

        if(isRefillingDeposit)
            RefillFuel();

    }

    public void UpdateRefillQuantity()
    {
        refillQuantity = maxFuel - fuel;
    }

    private void RefillFuel()
    {
        timer += Time.deltaTime;
        if (fuel < maxFuel)
        {
            float refillTime = Random.Range(minRefillSeconds / 10, maxRefillSeconds / 10);
            if (timer >= refillTime)
            {
                float refill = refillQuantity / (refillTime * 10);
                fuel += refill;
                timer = 0;
            }
        } else
        {
            isRefillingDeposit = false;
            fuel = maxFuel;
            depositEmpty = false;
        }
    }

    public bool IsRefillingDeposit()
    {
        return isRefillingDeposit;
    }

    public void RefillTank()
    {
        isRefillingDeposit = true;
    }

    public void StopRefillTank()
    {
        isRefillingDeposit = false;
    }

    public float GetCurrentFuel()
    {
        return fuel;
    }

    public float GetMaxFuel()
    {
        return maxFuel;
    }

    private void ConsumeFuel()
    {
        if (!depositEmpty)
        {
            
            timer += Time.deltaTime;
            if (timer >= secondsPerConsume)
            {
                fuel -= consumePerTime;
                timer = 0;
                if (fuel <= 0)
                    depositEmpty = true;
            }
        } 
        if (fuel <= 0)
        {
            depositEmpty = true;
            navMeshAgent.isStopped = true;
        }
    }
}
