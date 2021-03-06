﻿using System;
using Burrow.RPC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

// ReSharper disable InconsistentNaming
namespace Burrow.Tests.RPC.RpcRouteFinderAdapterTests
{
    [TestClass]
    public class MethodFindQueueName
    {
        private IRpcRouteFinder _routeFinder;

        [TestInitializeAttribute]
        public void Init()
        {
            _routeFinder = Substitute.For<IRpcRouteFinder>();
            _routeFinder.RequestExchangeName.Returns("RequestExchange");
            _routeFinder.RequestQueue.Returns("RequestQueue");
            _routeFinder.UniqueResponseQueue.Returns("ResposeQueue");
        }

        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void Should_throw_exception_if_type_is_not_request_nor_response()
        {
            // Arrange
            var adapter = new RpcRouteFinderAdapter(_routeFinder);

            // Action
            adapter.FindQueueName<SomeMessage>("");
        }


        [TestMethod]
        public void Should_return_valid_string_if_type_is_Request_or_Response()
        {
            // Arrange
            var adapter = new RpcRouteFinderAdapter(_routeFinder);

            // Action
            var requestQueue = adapter.FindQueueName<RpcRequest>("UnitTest");
            var responseQueue = adapter.FindQueueName<RpcResponse>("UnitTest");

            // Assert
            Assert.AreEqual("RequestQueue", requestQueue);
            Assert.AreEqual("ResposeQueue", responseQueue);

        }
    }
}
// ReSharper restore InconsistentNaming