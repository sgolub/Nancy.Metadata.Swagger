﻿using System;
using Nancy.Metadata.Modules;
using Nancy.Metadata.Swagger.Core;
using Nancy.Metadata.Swagger.DemoApplication.Model;
using Nancy.Metadata.Swagger.Fluent;
using Nancy.ModelBinding;

namespace Nancy.Metadata.Swagger.DemoApplication.Modules
{
    public class RootModule : NancyModule
    {
        public RootModule() : base("/api")
        {
            Get["SimpleRequest", "/hello"] = r => HelloWorld();
            Get["SimpleRequestWithParameter", "/hello/{name}"] = r => Hello(r.name);
            Get["SimpleRequestWithGenericModel", "/hello/generic"] = r => GenericHelloModel();

            Post["SimplePostRequst", "/hello"] = r => HelloPost();
            Post["PostRequestWithModel", "/hello/model"] = r => HelloModel();

            Post["PostRequestWithNestedModel", "/hello/nestedmodel"] = r => HelloNestedModel();
        }

        private dynamic GenericHelloModel()
        {
            GenericResponseModel<SimpleResponseModel> response = new GenericResponseModel<SimpleResponseModel>
            {
                Child = new SimpleResponseModel
                {
                    Hello = "Hello Post!"
                }
            };

            return Response.AsJson(response);
        }

        private Response HelloNestedModel()
        {
            NestedRequestModel model = this.Bind<NestedRequestModel>();

            SimpleResponseModel response = new SimpleResponseModel
            {
                Hello = string.Format("Hello, {0}. We got your name from nested obejct", model.SimpleModel.Name)
            };

            return Response.AsJson(response);
        }

        private Response HelloModel()
        {
            SimpleRequestModel model = this.Bind<SimpleRequestModel>();

            SimpleResponseModel response = new SimpleResponseModel
            {
                Hello = string.Format("Hello, {0}", model.Name)
            };

            return Response.AsJson(response);
        }

        private Response HelloPost()
        {
            SimpleResponseModel response = new SimpleResponseModel
            {
                Hello = "Hello Post!"
            };

            return Response.AsJson(response);
        }

        private Response Hello(string name)
        {
            SimpleResponseModel response = new SimpleResponseModel
            {
                Hello = string.Format("Hello, {0}", name)
            };

            return Response.AsJson(response);
        }

        private Response HelloWorld()
        {
            SimpleResponseModel response = new SimpleResponseModel
            {
                Hello = "Hello World!"
            };

            return Response.AsJson(response);
        }
    }

    public class RootMetadataModule : MetadataModule<SwaggerRouteMetadata>
    {
        public RootMetadataModule()
        {
            Describe["SimpleRequest"] = desc => new SwaggerRouteMetadata(desc)
                .With(i => i.WithResponseModel("200", typeof(SimpleResponseModel), "Sample response")
                            .WithSummary("Simple GET example"));

            Describe["SimpleRequestWithParameter"] = desc => new SwaggerRouteMetadata(desc)
                .With(i => i.WithResponseModel("200", typeof(SimpleResponseModel), "Sample response")
                            .WithRequestParameter("name")
                            .WithSummary("Simple GET with generic model"));

            Describe["SimpleRequestWithGenericModel"] = desc => new SwaggerRouteMetadata(desc)
                .With(i => i.WithResponseModel("200", typeof(GenericResponseModel<SimpleResponseModel>), "Sample response")
                            .WithRequestParameter("name")
                            .WithSummary("Simple GET with parameters"));

            Describe["SimplePostRequst"] = desc => new SwaggerRouteMetadata(desc)
                .With(info => info.WithResponseModel("200", typeof(SimpleResponseModel), "Sample response")
                    .WithSummary("Simple POST example"));

            Describe["PostRequestWithModel"] = desc => new SwaggerRouteMetadata(desc)
                .With(info => info.WithResponseModel("200", typeof(SimpleResponseModel))
                    .WithResponse("400", "Bad request")
                    .WithSummary("Simple POST example with request model")
                    .WithRequestModel(typeof(SimpleRequestModel)));

            Describe["PostRequestWithNestedModel"] = desc => new SwaggerRouteMetadata(desc)
                .With(info => info.WithResponseModel("200", typeof(SimpleResponseModel))
                    .WithResponse("400", "Bad request")
                    .WithSummary("Simple POST example with nested request model")
                    .WithRequestModel(typeof(NestedRequestModel)));
        }
    }
}