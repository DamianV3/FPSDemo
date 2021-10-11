using UnityEngine;
using Unity.Entities;
using Unity.Animation;
using Unity.DataFlowGraph;

public struct PlayerAnimationStateComponent : ISystemStateComponentData
{
    public GraphHandle Graph;
    public NodeHandle<ClipPlayerNode> ClipPlayerNode;
}

public class PlayerAnimationSystem : SystemBase
{
    PreAnimationGraphSystem m_GraphSystem;
    EndSimulationEntityCommandBufferSystem m_ECBSystem;
    EntityQuery m_AnimationDataQuery;

    protected override void OnCreate()
    {
        base.OnCreate();

        m_GraphSystem = World.GetOrCreateSystem<PreAnimationGraphSystem>();
        m_GraphSystem.AddRef();

        m_ECBSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

        m_AnimationDataQuery = GetEntityQuery(new EntityQueryDesc()
        {
            None = new ComponentType[] { typeof(PlayerAnimationData) },
            All = new ComponentType[] { typeof(PlayerAnimationStateComponent) }
        });
    }

    protected override void OnDestroy()
    {
        if (m_GraphSystem == null)
            return;

        m_GraphSystem.RemoveRef();
        base.OnDestroy();
    }

    protected override void OnUpdate()
    {
        CompleteDependency();

        var ecb = m_ECBSystem.CreateCommandBuffer();
        
        // Create graph for entities that have a PlayClipComponent but no graph (PlayClipStateComponent)
        Entities
            .WithName("CreateGraph")
            .WithNone<PlayerAnimationStateComponent>()
            .WithoutBurst()
            .WithStructuralChanges()
            .ForEach((Entity e, ref Rig rig, ref PlayerAnimationData animation) =>
            {
                var state = CreateGraph(e, m_GraphSystem, ref rig, ref animation);
                ecb.AddComponent(e, state);
                UnityEngine.Debug.Log("Create "  + rig.Value);
            }).Run();

        // Update graph if the animation component changed
        Entities
            .WithName("UpdateGraph")
            .WithChangeFilter<PlayerAnimationData>()
            .WithoutBurst()
            .ForEach((Entity e, ref Rig rig, ref PlayerAnimationData animation, ref PlayerAnimationStateComponent state) =>
            {
                UnityEngine.Debug.Log("Update" + rig.Value);
                m_GraphSystem.Set.SendMessage(state.ClipPlayerNode, ClipPlayerNode.SimulationPorts.Clip, animation.Clip);
            }).Run();

        // Destroy graph for which the entity is missing the PlayClipComponent
        Entities
            .WithName("DestroyGraph")
            .WithNone<PlayerAnimationData>()
            .WithoutBurst()
            .WithStructuralChanges()
            .ForEach((Entity e, ref PlayerAnimationStateComponent state) =>
            {
                UnityEngine.Debug.Log("Dispose");
                m_GraphSystem.Dispose(state.Graph);
            }).Run();

        if (m_AnimationDataQuery.CalculateEntityCount() > 0)
            ecb.RemoveComponent(m_AnimationDataQuery, typeof(PlayerAnimationStateComponent));
    }

    static PlayerAnimationStateComponent CreateGraph(
        Entity entity,
        PreAnimationGraphSystem graphSystem,
        ref Rig rig,
        ref PlayerAnimationData playClip
    )
    {
        GraphHandle graph = graphSystem.CreateGraph();
        var data = new PlayerAnimationStateComponent
        {
            Graph = graph,
            ClipPlayerNode = graphSystem.CreateNode<ClipPlayerNode>(graph)
        };

        var deltaTimeNode = graphSystem.CreateNode<ConvertDeltaTimeToFloatNode>(graph);
        var entityNode = graphSystem.CreateNode(graph, entity);

        var set = graphSystem.Set;

        // Connect kernel ports
        set.Connect(entityNode, deltaTimeNode, ConvertDeltaTimeToFloatNode.KernelPorts.Input);
        set.Connect(deltaTimeNode, ConvertDeltaTimeToFloatNode.KernelPorts.Output, data.ClipPlayerNode, ClipPlayerNode.KernelPorts.DeltaTime);
        set.Connect(data.ClipPlayerNode, ClipPlayerNode.KernelPorts.Output, entityNode, NodeSet.ConnectionType.Feedback);

        // Send messages to set parameters on the ClipPlayerNode
        set.SetData(data.ClipPlayerNode, ClipPlayerNode.KernelPorts.Speed, 1.0f);
        set.SendMessage(data.ClipPlayerNode, ClipPlayerNode.SimulationPorts.Configuration, new ClipConfiguration { Mask = ClipConfigurationMask.LoopTime });
        set.SendMessage(data.ClipPlayerNode, ClipPlayerNode.SimulationPorts.Rig, rig);
        set.SendMessage(data.ClipPlayerNode, ClipPlayerNode.SimulationPorts.Clip, playClip.Clip);

        return data;
    }
}
