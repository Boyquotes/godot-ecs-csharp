using System;
using GdEcs;
using Godot;

public class Example1 : Node
{

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (Input.IsActionJustPressed("ui_accept"))
        {
            var dude = GetNode<EntityKinematicBody2D>("%DudeEntity");
            if (dude.ComponentStore.HasComponentsOfType<FourDirectionalUserInputComponent>())
            {
                dude.RemoveComponent(dude.ComponentStore.GetFirstComponentOfType<FourDirectionalUserInputComponent>()!);
            }
            else
            {
                dude.AddComponent(new FourDirectionalUserInputComponent());
            }
        }

        if (Input.IsActionJustPressed("ui_cancel"))
        {
            var player = GetNode<EntityKinematicBody2D>("%PlayerEntity");
            var dude = GetNode<EntityKinematicBody2D>("%DudeEntity");
            var entities = GetNode<Node>("%Entities");

            var newDude = (EntityKinematicBody2D)dude.Duplicate();
            newDude.Position = player.Position + new Vector2(75, 0);
            // var velComp = new Velocity2DComponent();
            // velComp.Velocity = player.ComponentStore.GetFirstComponentOfType<Velocity2DComponent>()!.Velocity;
            // newDude.AddComponent(velComp);
            entities.AddChild(newDude);
        }
    }

}
