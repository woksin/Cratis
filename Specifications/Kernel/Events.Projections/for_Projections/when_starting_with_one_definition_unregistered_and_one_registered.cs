// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Events.Projections.Definitions;
using Cratis.Events.Projections.Pipelines;
using Cratis.Properties;

namespace Cratis.Events.Projections.for_Projections
{
    public class when_starting_with_one_definition_unregistered_and_one_registered : given.no_projections
    {
        ProjectionId unregistered_projection_identifier = "793f9d99-bfcc-4d85-88f7-965beed001e7";
        ProjectionDefinition unregistered_projection_definition;
        ProjectionPipelineDefinition unregistered_pipeline_definition;
        IProjectionPipeline pipeline_registered;

        Mock<IProjection> unregistered_projection;
        Mock<IProjectionPipeline> unregistered_pipeline;

        async Task Establish()
        {
            unregistered_projection_definition = new(
                unregistered_projection_identifier,
                "My Unregistered Projection",
                new ModelDefinition("Some Model", "{}"),
                new Dictionary<EventType, FromDefinition>(),
                new Dictionary<PropertyPath, ChildrenDefinition>(),
                null);

            unregistered_pipeline_definition = new(
                unregistered_projection_identifier,
                "ce35564f-f75c-4429-b91e-b236f48dd88b",
                Array.Empty<ProjectionResultStoreDefinition>());

            pipeline_definitions.Setup(_ => _.GetAll()).Returns(new[] { unregistered_pipeline_definition });
            projection_definitions.Setup(_ => _.HasFor(unregistered_projection_identifier)).Returns(Task.FromResult(true));
            projection_definitions.Setup(_ => _.GetFor(unregistered_projection_identifier)).Returns(Task.FromResult(unregistered_projection_definition));

            await projections.Register(projection_definition, pipeline_definition);

            unregistered_projection = new();
            unregistered_projection.SetupGet(_ => _.Identifier).Returns(unregistered_projection_identifier);
            unregistered_pipeline = new();
            projection_factory.Setup(_ => _.CreateFrom(unregistered_projection_definition)).Returns(unregistered_projection.Object);
            pipeline_factory.Setup(_ => _.CreateFrom(unregistered_projection.Object, unregistered_pipeline_definition)).Returns(unregistered_pipeline.Object);

            projections.Pipelines.Subscribe(_ => pipeline_registered = _);
        }

        void Because() => projections.Start();

        [Fact] void should_register_projection_definition() => projection_definitions.Verify(_ => _.Register(unregistered_projection_definition), Once());
        [Fact] void should_register_pipeline_definition() => pipeline_definitions.Verify(_ => _.Register(unregistered_pipeline_definition), Once());
        [Fact] void should_make_pipeline_the_next_in_observable() => pipeline_registered.ShouldEqual(unregistered_pipeline.Object);
        [Fact] void should_register_pipeline() => projections.GetPipelines().ToArray()[1].ShouldEqual(unregistered_pipeline.Object);
    }
}