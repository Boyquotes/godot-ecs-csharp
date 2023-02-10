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
    }

}
