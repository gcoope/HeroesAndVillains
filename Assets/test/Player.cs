using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public float moveSpeed = 3.0f;
    public GameObject bulletPrefab = null; // this prefab will be instantiated on the server

    private Vector3 _velocity;

    // Command means when the client call this function, it's "forwarded" to the server
    // and the code will actually run on server side
    // NOTE: the function name has to start with "Cmd"
    [Command]
    public void Cmd_Shoot()
    {
        // create server-side instance
        GameObject obj = (GameObject)Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        // setup bullet component
        Bullet bullet = obj.GetComponent<Bullet>();
        bullet.velocity = transform.forward;
        // destroy after 2 secs
        Destroy(obj, 2.0f);
        // spawn on the clients
        NetworkServer.Spawn(obj);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        // you can use this function to do things like create camera, audio listeners, etc.
        // so things which has to be done only for our player
    }

    private void Update()
    {
        // isLocalPlayer is true for the client who "owns" the player object
        // we only want input handling for our player
        if (!base.isLocalPlayer)
            return;

        // handle input here...

        _velocity = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            ++_velocity.z;
        if (Input.GetKey(KeyCode.S))
            --_velocity.z;
        if (Input.GetKey(KeyCode.A))
            --_velocity.x;
        if (Input.GetKey(KeyCode.D))
            ++_velocity.x;

        _velocity.Normalize();

        // when pressing the space button, ask the server to spawn a bullet
        if (Input.GetKeyDown(KeyCode.Space))
            Cmd_Shoot();
    }

    private void FixedUpdate()
    {
        // because Local Player Authority is true, the client has to move the player
        // only the resulting transform will be sent to the server and to the other clients
        if (!base.isLocalPlayer)
            return;

        // if that flag is not true, we should check that the code runs only on server
        // it could be done by checking base.isServer

        // transforming and other authoritive stuff here...

        transform.position += _velocity * Time.deltaTime * moveSpeed;
    }
}
