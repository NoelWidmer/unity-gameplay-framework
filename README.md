# Unity3D Gameplay Framework

## Introduction

All objects spawned in a Unity scene are <code>Behaviour</code>s. For local games we mostly work with <code>MonoBehaviour</code>s and for networking games Unity provides us <code>NetworkBehaviour</code>s. Aside from those there aren't many abstractions where Unity supports us. We basically get <code>Behaviour</code>s that support things that we often don't need (like a <code>Transform</code>) and don't get other things that would really help. I came to the conclusion that great gameplay support should be a feature that ships with the engine, finding myself abandoning projects because it doesn't.<br>

I've worked with Unreal's Gameplay Framework which I really like and therefore want to improve Unity's gameplay layer by adding some of the things I have learned from UE4 (Unreal Engine 4). However, the existing <code>Behaviour</code> code base cannot be removed as it lies at the heart of the Unity Engine.

## The Gameplay Framework Classes

The GF (Gameplay Framework) consist of a few classes that help to structure gameplay logic. 

![Class Relationship Diagram](./docs/ClassRelationshipDiagram.png)

More content coming soon
