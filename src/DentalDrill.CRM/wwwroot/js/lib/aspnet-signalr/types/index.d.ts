declare namespace signalR {
    /** @private */
    class AbortController implements AbortSignal {
        private isAborted;
        onabort: () => void;
        abort(): void;
        readonly signal: AbortSignal;
        readonly aborted: boolean;
    }

    /** Represents a signal that can be monitored to determine if a request has been aborted. */
    interface AbortSignal {
        /** Indicates if the request has been aborted. */
        aborted: boolean;
        /** Set this to a handler that will be invoked when the request is aborted. */
        onabort: () => void;
    }

    /** Error thrown when an HTTP request fails. */
    class HttpError extends Error {
        private __proto__;
        /** The HTTP status code represented by this error. */
        statusCode: number;
        /** Constructs a new instance of {@link @aspnet/signalr.HttpError}.
         *
         * @param {string} errorMessage A descriptive error message.
         * @param {number} statusCode The HTTP status code represented by this error.
         */
        constructor(errorMessage: string, statusCode: number);
    }

    /** Error thrown when a timeout elapses. */
    class TimeoutError extends Error {
        private __proto__;
        /** Constructs a new instance of {@link @aspnet/signalr.TimeoutError}.
         *
         * @param {string} errorMessage A descriptive error message.
         */
        constructor(errorMessage?: string);
    }

    /** @private */
    interface HandshakeRequestMessage {
        readonly protocol: string;
        readonly version: number;
    }

    /** @private */
    interface HandshakeResponseMessage {
        readonly error: string;
    }

    /** @private */
    class HandshakeProtocol {
        writeHandshakeRequest(handshakeRequest: HandshakeRequestMessage): string;
        parseHandshakeResponse(data: any): [any, HandshakeResponseMessage];
    }

    /** Represents an HTTP request. */
    interface HttpRequest {
        /** The HTTP method to use for the request. */
        method?: string;
        /** The URL for the request. */
        url?: string;
        /** The body content for the request. May be a string or an ArrayBuffer (for binary data). */
        content?: string | ArrayBuffer;
        /** An object describing headers to apply to the request. */
        headers?: {
            [key: string]: string;
        };
        /** The XMLHttpRequestResponseType to apply to the request. */
        responseType?: XMLHttpRequestResponseType;
        /** An AbortSignal that can be monitored for cancellation. */
        abortSignal?: AbortSignal;
        /** The time to wait for the request to complete before throwing a TimeoutError. Measured in milliseconds. */
        timeout?: number;
    }

    /** Represents an HTTP response. */
    class HttpResponse {
        readonly statusCode: number;
        readonly statusText?: string;
        readonly content?: string | ArrayBuffer;
        /** Constructs a new instance of {@link @aspnet/signalr.HttpResponse} with the specified status code.
         *
         * @param {number} statusCode The status code of the response.
         */
        constructor(statusCode: number);
        /** Constructs a new instance of {@link @aspnet/signalr.HttpResponse} with the specified status code and message.
         *
         * @param {number} statusCode The status code of the response.
         * @param {string} statusText The status message of the response.
         */
        constructor(statusCode: number, statusText: string);
        /** Constructs a new instance of {@link @aspnet/signalr.HttpResponse} with the specified status code, message and string content.
         *
         * @param {number} statusCode The status code of the response.
         * @param {string} statusText The status message of the response.
         * @param {string} content The content of the response.
         */
        constructor(statusCode: number, statusText: string, content: string);
        /** Constructs a new instance of {@link @aspnet/signalr.HttpResponse} with the specified status code, message and binary content.
         *
         * @param {number} statusCode The status code of the response.
         * @param {string} statusText The status message of the response.
         * @param {ArrayBuffer} content The content of the response.
         */
        constructor(statusCode: number, statusText: string, content: ArrayBuffer);
    }

    /** Abstraction over an HTTP client.
     *
     * This class provides an abstraction over an HTTP client so that a different implementation can be provided on different platforms.
     */
    abstract class HttpClient {
        /** Issues an HTTP GET request to the specified URL, returning a Promise that resolves with an {@link @aspnet/signalr.HttpResponse} representing the result.
         *
         * @param {string} url The URL for the request.
         * @returns {Promise<HttpResponse>} A Promise that resolves with an {@link @aspnet/signalr.HttpResponse} describing the response, or rejects with an Error indicating a failure.
         */
        get(url: string): Promise<HttpResponse>;
        /** Issues an HTTP GET request to the specified URL, returning a Promise that resolves with an {@link @aspnet/signalr.HttpResponse} representing the result.
         *
         * @param {string} url The URL for the request.
         * @param {HttpRequest} options Additional options to configure the request. The 'url' field in this object will be overridden by the url parameter.
         * @returns {Promise<HttpResponse>} A Promise that resolves with an {@link @aspnet/signalr.HttpResponse} describing the response, or rejects with an Error indicating a failure.
         */
        get(url: string, options: HttpRequest): Promise<HttpResponse>;
        /** Issues an HTTP POST request to the specified URL, returning a Promise that resolves with an {@link @aspnet/signalr.HttpResponse} representing the result.
         *
         * @param {string} url The URL for the request.
         * @returns {Promise<HttpResponse>} A Promise that resolves with an {@link @aspnet/signalr.HttpResponse} describing the response, or rejects with an Error indicating a failure.
         */
        post(url: string): Promise<HttpResponse>;
        /** Issues an HTTP POST request to the specified URL, returning a Promise that resolves with an {@link @aspnet/signalr.HttpResponse} representing the result.
         *
         * @param {string} url The URL for the request.
         * @param {HttpRequest} options Additional options to configure the request. The 'url' field in this object will be overridden by the url parameter.
         * @returns {Promise<HttpResponse>} A Promise that resolves with an {@link @aspnet/signalr.HttpResponse} describing the response, or rejects with an Error indicating a failure.
         */
        post(url: string, options: HttpRequest): Promise<HttpResponse>;
        /** Issues an HTTP DELETE request to the specified URL, returning a Promise that resolves with an {@link @aspnet/signalr.HttpResponse} representing the result.
         *
         * @param {string} url The URL for the request.
         * @returns {Promise<HttpResponse>} A Promise that resolves with an {@link @aspnet/signalr.HttpResponse} describing the response, or rejects with an Error indicating a failure.
         */
        delete(url: string): Promise<HttpResponse>;
        /** Issues an HTTP DELETE request to the specified URL, returning a Promise that resolves with an {@link @aspnet/signalr.HttpResponse} representing the result.
         *
         * @param {string} url The URL for the request.
         * @param {HttpRequest} options Additional options to configure the request. The 'url' field in this object will be overridden by the url parameter.
         * @returns {Promise<HttpResponse>} A Promise that resolves with an {@link @aspnet/signalr.HttpResponse} describing the response, or rejects with an Error indicating a failure.
         */
        delete(url: string, options: HttpRequest): Promise<HttpResponse>;
        /** Issues an HTTP request to the specified URL, returning a {@link Promise} that resolves with an {@link @aspnet/signalr.HttpResponse} representing the result.
         *
         * @param {HttpRequest} request An {@link @aspnet/signalr.HttpRequest} describing the request to send.
         * @returns {Promise<HttpResponse>} A Promise that resolves with an HttpResponse describing the response, or rejects with an Error indicating a failure.
         */
        abstract send(request: HttpRequest): Promise<HttpResponse>;
    }

    /** Default implementation of {@link @aspnet/signalr.HttpClient}. */
    class DefaultHttpClient extends HttpClient {
        private readonly logger;
        /** Creates a new instance of the {@link @aspnet/signalr.DefaultHttpClient}, using the provided {@link @aspnet/signalr.ILogger} to log messages. */
        constructor(logger: ILogger);
        /** @inheritDoc */
        send(request: HttpRequest): Promise<HttpResponse>;
    }

    /** @private */
    interface INegotiateResponse {
        connectionId?: string;
        availableTransports?: IAvailableTransport[];
        url?: string;
        accessToken?: string;
    }
    /** @private */
    interface IAvailableTransport {
        transport: keyof typeof HttpTransportType;
        transferFormats: Array<keyof typeof TransferFormat>;
    }
    /** @private */
    class HttpConnection implements IConnection {
        private connectionState;
        private baseUrl;
        private readonly httpClient;
        private readonly logger;
        private readonly options;
        private transport;
        private startPromise;
        private stopError?;
        private accessTokenFactory?;
        readonly features: any;
        onreceive: (data: string | ArrayBuffer) => void;
        onclose: (e?: Error) => void;
        constructor(url: string, options?: IHttpConnectionOptions);
        start(): Promise<void>;
        start(transferFormat: TransferFormat): Promise<void>;
        send(data: string | ArrayBuffer): Promise<void>;
        stop(error?: Error): Promise<void>;
        private startInternal;
        private getNegotiationResponse;
        private createConnectUrl;
        private createTransport;
        private constructTransport;
        private resolveTransport;
        private isITransport;
        private changeState;
        private stopConnection;
        private resolveUrl;
        private resolveNegotiateUrl;
    }

    /** Represents a connection to a SignalR Hub. */
    class HubConnection {
        private readonly connection;
        private readonly logger;
        private protocol;
        private handshakeProtocol;
        private callbacks;
        private methods;
        private id;
        private closedCallbacks;
        private timeoutHandle?;
        private receivedHandshakeResponse;
        /** The server timeout in milliseconds.
         *
         * If this timeout elapses without receiving any messages from the server, the connection will be terminated with an error.
         * The default timeout value is 30,000 milliseconds (30 seconds).
         */
        serverTimeoutInMilliseconds: number;
        private constructor();
        /** Starts the connection.
         *
         * @returns {Promise<void>} A Promise that resolves when the connection has been successfully established, or rejects with an error.
         */
        start(): Promise<void>;
        /** Stops the connection.
         *
         * @returns {Promise<void>} A Promise that resolves when the connection has been successfully terminated, or rejects with an error.
         */
        stop(): Promise<void>;
        /** Invokes a streaming hub method on the server using the specified name and arguments.
         *
         * @typeparam T The type of the items returned by the server.
         * @param {string} methodName The name of the server method to invoke.
         * @param {any[]} args The arguments used to invoke the server method.
         * @returns {IStreamResult<T>} An object that yields results from the server as they are received.
         */
        stream<T = any>(methodName: string, ...args: any[]): IStreamResult<T>;
        /** Invokes a hub method on the server using the specified name and arguments. Does not wait for a response from the receiver.
         *
         * The Promise returned by this method resolves when the client has sent the invocation to the server. The server may still
         * be processing the invocation.
         *
         * @param {string} methodName The name of the server method to invoke.
         * @param {any[]} args The arguments used to invoke the server method.
         * @returns {Promise<void>} A Promise that resolves when the invocation has been successfully sent, or rejects with an error.
         */
        send(methodName: string, ...args: any[]): Promise<void>;
        /** Invokes a hub method on the server using the specified name and arguments.
         *
         * The Promise returned by this method resolves when the server indicates it has finished invoking the method. When the promise
         * resolves, the server has finished invoking the method. If the server method returns a result, it is produced as the result of
         * resolving the Promise.
         *
         * @typeparam T The expected return type.
         * @param {string} methodName The name of the server method to invoke.
         * @param {any[]} args The arguments used to invoke the server method.
         * @returns {Promise<T>} A Promise that resolves with the result of the server method (if any), or rejects with an error.
         */
        invoke<T = any>(methodName: string, ...args: any[]): Promise<T>;
        /** Registers a handler that will be invoked when the hub method with the specified method name is invoked.
         *
         * @param {string} methodName The name of the hub method to define.
         * @param {Function} newMethod The handler that will be raised when the hub method is invoked.
         */
        on(methodName: string, newMethod: (...args: any[]) => void): void;
        /** Removes all handlers for the specified hub method.
         *
         * @param {string} methodName The name of the method to remove handlers for.
         */
        off(methodName: string): void;
        /** Removes the specified handler for the specified hub method.
         *
         * You must pass the exact same Function instance as was previously passed to {@link @aspnet/signalr.HubConnection.on}. Passing a different instance (even if the function
         * body is the same) will not remove the handler.
         *
         * @param {string} methodName The name of the method to remove handlers for.
         * @param {Function} method The handler to remove. This must be the same Function instance as the one passed to {@link @aspnet/signalr.HubConnection.on}.
         */
        off(methodName: string, method: (...args: any[]) => void): void;
        /** Registers a handler that will be invoked when the connection is closed.
         *
         * @param {Function} callback The handler that will be invoked when the connection is closed. Optionally receives a single argument containing the error that caused the connection to close (if any).
         */
        onclose(callback: (error?: Error) => void): void;
        private processIncomingData;
        private processHandshakeResponse;
        private configureTimeout;
        private serverTimeout;
        private invokeClientMethod;
        private connectionClosed;
        private cleanupTimeout;
        private createInvocation;
        private createStreamInvocation;
        private createCancelInvocation;
    }

    /** A builder for configuring {@link @aspnet/signalr.HubConnection} instances. */
    class HubConnectionBuilder {
        /** Configures console logging for the {@link @aspnet/signalr.HubConnection}.
         *
         * @param {LogLevel} logLevel The minimum level of messages to log. Anything at this level, or a more severe level, will be logged.
         * @returns The {@link @aspnet/signalr.HubConnectionBuilder} instance, for chaining.
         */
        configureLogging(logLevel: LogLevel): HubConnectionBuilder;
        /** Configures custom logging for the {@link @aspnet/signalr.HubConnection}.
         *
         * @param {ILogger} logger An object implementing the {@link @aspnet/signalr.ILogger} interface, which will be used to write all log messages.
         * @returns The {@link @aspnet/signalr.HubConnectionBuilder} instance, for chaining.
         */
        configureLogging(logger: ILogger): HubConnectionBuilder;
        /** Configures the {@link @aspnet/signalr.HubConnection} to use HTTP-based transports to connect to the specified URL.
         *
         * The transport will be selected automatically based on what the server and client support.
         *
         * @param {string} url The URL the connection will use.
         * @returns The {@link @aspnet/signalr.HubConnectionBuilder} instance, for chaining.
         */
        withUrl(url: string): HubConnectionBuilder;
        /** Configures the {@link @aspnet/signalr.HubConnection} to use the specified HTTP-based transport to connect to the specified URL.
         *
         * @param {string} url The URL the connection will use.
         * @param {HttpTransportType} transportType The specific transport to use.
         * @returns The {@link @aspnet/signalr.HubConnectionBuilder} instance, for chaining.
         */
        withUrl(url: string, transportType: HttpTransportType): HubConnectionBuilder;
        /** Configures the {@link @aspnet/signalr.HubConnection} to use HTTP-based transports to connect to the specified URL.
         *
         * @param {string} url The URL the connection will use.
         * @param {IHttpConnectionOptions} options An options object used to configure the connection.
         * @returns The {@link @aspnet/signalr.HubConnectionBuilder} instance, for chaining.
         */
        withUrl(url: string, options: IHttpConnectionOptions): HubConnectionBuilder;
        /** Configures the {@link @aspnet/signalr.HubConnection} to use the specified Hub Protocol.
         *
         * @param {IHubProtocol} protocol The {@link @aspnet/signalr.IHubProtocol} implementation to use.
         */
        withHubProtocol(protocol: IHubProtocol): HubConnectionBuilder;
        /** Creates a {@link @aspnet/signalr.HubConnection} from the configuration options specified in this builder.
         *
         * @returns {HubConnection} The configured {@link @aspnet/signalr.HubConnection}.
         */
        build(): HubConnection;
    }

    /** @private */
    interface IConnection {
        readonly features: any;
        start(transferFormat: TransferFormat): Promise<void>;
        send(data: string | ArrayBuffer): Promise<void>;
        stop(error?: Error): Promise<void>;
        onreceive: (data: string | ArrayBuffer) => void;
        onclose: (error?: Error) => void;
    }

    /** Options provided to the 'withUrl' method on {@link @aspnet/signalr.HubConnectionBuilder} to configure options for the HTTP-based transports. */
    interface IHttpConnectionOptions {
        /** An {@link @aspnet/signalr.HttpClient} that will be used to make HTTP requests. */
        httpClient?: HttpClient;
        /** An {@link @aspnet/signalr.HttpTransportType} value specifying the transport to use for the connection. */
        transport?: HttpTransportType | ITransport;
        /** Configures the logger used for logging.
         *
         * Provide an {@link @aspnet/signalr.ILogger} instance, and log messages will be logged via that instance. Alternatively, provide a value from
         * the {@link @aspnet/signalr.LogLevel} enumeration and a default logger which logs to the Console will be configured to log messages of the specified
         * level (or higher).
         */
        logger?: ILogger | LogLevel;
        /** A function that provides an access token required for HTTP Bearer authentication.
         *
         * @returns {string | Promise<string>} A string containing the access token, or a Promise that resolves to a string containing the access token.
         */
        accessTokenFactory?(): string | Promise<string>;
        /** A boolean indicating if message content should be logged.
         *
         * Message content can contain sensitive user data, so this is disabled by default.
         */
        logMessageContent?: boolean;
        /** A boolean indicating if negotiation should be skipped.
         *
         * Negotiation can only be skipped when the {@link @aspnet/signalr.IHttpConnectionOptions.transport} property is set to 'HttpTransportType.WebSockets'.
         */
        skipNegotiation?: boolean;
    }

    /** Defines the type of a Hub Message. */
    enum MessageType {
        /** Indicates the message is an Invocation message and implements the {@link @aspnet/signalr.InvocationMessage} interface. */
        Invocation = 1,
        /** Indicates the message is a StreamItem message and implements the {@link @aspnet/signalr.StreamItemMessage} interface. */
        StreamItem = 2,
        /** Indicates the message is a Completion message and implements the {@link @aspnet/signalr.CompletionMessage} interface. */
        Completion = 3,
        /** Indicates the message is a Stream Invocation message and implements the {@link @aspnet/signalr.StreamInvocationMessage} interface. */
        StreamInvocation = 4,
        /** Indicates the message is a Cancel Invocation message and implements the {@link @aspnet/signalr.CancelInvocationMessage} interface. */
        CancelInvocation = 5,
        /** Indicates the message is a Ping message and implements the {@link @aspnet/signalr.PingMessage} interface. */
        Ping = 6,
        /** Indicates the message is a Close message and implements the {@link @aspnet/signalr.CloseMessage} interface. */
        Close = 7
    }

    /** Defines a dictionary of string keys and string values representing headers attached to a Hub message. */
    interface MessageHeaders {
        /** Gets or sets the header with the specified key. */
        [key: string]: string;
    }

    /** Union type of all known Hub messages. */
    type HubMessage = InvocationMessage | StreamInvocationMessage | StreamItemMessage | CompletionMessage | CancelInvocationMessage | PingMessage | CloseMessage;

    /** Defines properties common to all Hub messages. */
    interface HubMessageBase {
        /** A {@link @aspnet/signalr.MessageType} value indicating the type of this message. */
        readonly type: MessageType;
    }

    /** Defines properties common to all Hub messages relating to a specific invocation. */
    interface HubInvocationMessage extends HubMessageBase {
        /** A {@link @aspnet/signalr.MessageHeaders} dictionary containing headers attached to the message. */
        readonly headers?: MessageHeaders;
        /** The ID of the invocation relating to this message.
         *
         * This is expected to be present for {@link @aspnet/signalr.StreamInvocationMessage} and {@link @aspnet/signalr.CompletionMessage}. It may
         * be 'undefined' for an {@link @aspnet/signalr.InvocationMessage} if the sender does not expect a response.
         */
        readonly invocationId?: string;
    }

    /** A hub message representing a non-streaming invocation. */
    interface InvocationMessage extends HubInvocationMessage {
        /** @inheritDoc */
        readonly type: MessageType.Invocation;
        /** The target method name. */
        readonly target: string;
        /** The target method arguments. */
        readonly arguments: any[];
    }

    /** A hub message representing a streaming invocation. */
    interface StreamInvocationMessage extends HubInvocationMessage {
        /** @inheritDoc */
        readonly type: MessageType.StreamInvocation;
        /** The invocation ID. */
        readonly invocationId: string;
        /** The target method name. */
        readonly target: string;
        /** The target method arguments. */
        readonly arguments: any[];
    }

    /** A hub message representing a single item produced as part of a result stream. */
    interface StreamItemMessage extends HubInvocationMessage {
        /** @inheritDoc */
        readonly type: MessageType.StreamItem;
        /** The invocation ID. */
        readonly invocationId: string;
        /** The item produced by the server. */
        readonly item?: any;
    }

    /** A hub message representing the result of an invocation. */
    interface CompletionMessage extends HubInvocationMessage {
        /** @inheritDoc */
        readonly type: MessageType.Completion;
        /** The invocation ID. */
        readonly invocationId: string;
        /** The error produced by the invocation, if any.
         *
         * Either {@link @aspnet/signalr.CompletionMessage.error} or {@link @aspnet/signalr.CompletionMessage.result} must be defined, but not both.
         */
        readonly error?: string;
        /** The result produced by the invocation, if any.
         *
         * Either {@link @aspnet/signalr.CompletionMessage.error} or {@link @aspnet/signalr.CompletionMessage.result} must be defined, but not both.
         */
        readonly result?: any;
    }

    /** A hub message indicating that the sender is still active. */
    interface PingMessage extends HubMessageBase {
        /** @inheritDoc */
        readonly type: MessageType.Ping;
    }

    /** A hub message indicating that the sender is closing the connection.
     *
     * If {@link @aspnet/signalr.CloseMessage.error} is defined, the sender is closing the connection due to an error.
     */
    interface CloseMessage extends HubMessageBase {
        /** @inheritDoc */
        readonly type: MessageType.Close;
        /** The error that triggered the close, if any.
         *
         * If this property is undefined, the connection was closed normally and without error.
         */
        readonly error?: string;
    }

    /** A hub message sent to request that a streaming invocation be canceled. */
    interface CancelInvocationMessage extends HubInvocationMessage {
        /** @inheritDoc */
        readonly type: MessageType.CancelInvocation;
        /** The invocation ID. */
        readonly invocationId: string;
    }

    /** A protocol abstraction for communicating with SignalR Hubs.  */
    interface IHubProtocol {
        /** The name of the protocol. This is used by SignalR to resolve the protocol between the client and server. */
        readonly name: string;
        /** The version of the protocol. */
        readonly version: number;
        /** The {@link @aspnet/signalr.TransferFormat} of the protocol. */
        readonly transferFormat: TransferFormat;
        /** Creates an array of {@link @aspnet/signalr.HubMessage} objects from the specified serialized representation.
         *
         * If {@link @aspnet/signalr.IHubProtocol.transferFormat} is 'Text', the `input` parameter must be a string, otherwise it must be an ArrayBuffer.
         *
         * @param {string | ArrayBuffer} input A string, or ArrayBuffer containing the serialized representation.
         * @param {ILogger} logger A logger that will be used to log messages that occur during parsing.
         */
        parseMessages(input: string | ArrayBuffer, logger: ILogger): HubMessage[];
        /** Writes the specified {@link @aspnet/signalr.HubMessage} to a string or ArrayBuffer and returns it.
         *
         * If {@link @aspnet/signalr.IHubProtocol.transferFormat} is 'Text', the result of this method will be a string, otherwise it will be an ArrayBuffer.
         *
         * @param {HubMessage} message The message to write.
         * @returns {string | ArrayBuffer} A string or ArrayBuffer containing the serialized representation of the message.
         */
        writeMessage(message: HubMessage): string | ArrayBuffer;
    }

    /** Indicates the severity of a log message.
     *
     * Log Levels are ordered in increasing severity. So `Debug` is more severe than `Trace`, etc.
     */
    enum LogLevel {
        /** Log level for very low severity diagnostic messages. */
        Trace = 0,
        /** Log level for low severity diagnostic messages. */
        Debug = 1,
        /** Log level for informational diagnostic messages. */
        Information = 2,
        /** Log level for diagnostic messages that indicate a non-fatal problem. */
        Warning = 3,
        /** Log level for diagnostic messages that indicate a failure in the current operation. */
        Error = 4,
        /** Log level for diagnostic messages that indicate a failure that will terminate the entire application. */
        Critical = 5,
        /** The highest possible log level. Used when configuring logging to indicate that no log messages should be emitted. */
        None = 6
    }

    /** An abstraction that provides a sink for diagnostic messages. */
    interface ILogger {
        /** Called by the framework to emit a diagnostic message.
         *
         * @param {LogLevel} logLevel The severity level of the message.
         * @param {string} message The message.
         */
        log(logLevel: LogLevel, message: string): void;
    }

    /** Specifies a specific HTTP transport type. */
    enum HttpTransportType {
        /** Specifies no transport preference. */
        None = 0,
        /** Specifies the WebSockets transport. */
        WebSockets = 1,
        /** Specifies the Server-Sent Events transport. */
        ServerSentEvents = 2,
        /** Specifies the Long Polling transport. */
        LongPolling = 4
    }

    /** Specifies the transfer format for a connection. */
    enum TransferFormat {
        /** Specifies that only text data will be transmitted over the connection. */
        Text = 1,
        /** Specifies that binary data will be transmitted over the connection. */
        Binary = 2
    }

    /** An abstraction over the behavior of transports. This is designed to support the framework and not intended for use by applications. */
    interface ITransport {
        connect(url: string, transferFormat: TransferFormat): Promise<void>;
        send(data: any): Promise<void>;
        stop(): Promise<void>;
        onreceive: (data: string | ArrayBuffer) => void;
        onclose: (error?: Error) => void;
    }

    /** Implements the JSON Hub Protocol. */
    class JsonHubProtocol implements IHubProtocol {
        /** @inheritDoc */
        readonly name: string;
        /** @inheritDoc */
        readonly version: number;
        /** @inheritDoc */
        readonly transferFormat: TransferFormat;
        /** Creates an array of {@link @aspnet/signalr.HubMessage} objects from the specified serialized representation.
         *
         * @param {string} input A string containing the serialized representation.
         * @param {ILogger} logger A logger that will be used to log messages that occur during parsing.
         */
        parseMessages(input: string, logger: ILogger): HubMessage[];
        /** Writes the specified {@link @aspnet/signalr.HubMessage} to a string and returns it.
         *
         * @param {HubMessage} message The message to write.
         * @returns {string} A string containing the serialized representation of the message.
         */
        writeMessage(message: HubMessage): string;
        private isInvocationMessage;
        private isStreamItemMessage;
        private isCompletionMessage;
        private assertNotEmptyString;
    }

    /** A logger that does nothing when log messages are sent to it. */
    class NullLogger implements ILogger {
        /** The singleton instance of the {@link @aspnet/signalr.NullLogger}. */
        static instance: ILogger;
        private constructor();
        /** @inheritDoc */
        log(_logLevel: LogLevel, _message: string): void;
    }

    /** @private */
    class LongPollingTransport implements ITransport {
        private readonly httpClient;
        private readonly accessTokenFactory;
        private readonly logger;
        private readonly logMessageContent;
        private url;
        private pollAbort;
        private shutdownTimer;
        private shutdownTimeout;
        private running;
        private stopped;
        readonly pollAborted: boolean;
        constructor(httpClient: HttpClient, accessTokenFactory: () => string | Promise<string>, logger: ILogger, logMessageContent: boolean, shutdownTimeout?: number);
        connect(url: string, transferFormat: TransferFormat): Promise<void>;
        private updateHeaderToken;
        private poll;
        send(data: any): Promise<void>;
        stop(): Promise<void>;
        onreceive: (data: string | ArrayBuffer) => void;
        onclose: (error?: Error) => void;
    }

    /** @private */
    class ServerSentEventsTransport implements ITransport {
        private readonly httpClient;
        private readonly accessTokenFactory;
        private readonly logger;
        private readonly logMessageContent;
        private eventSource;
        private url;
        constructor(httpClient: HttpClient, accessTokenFactory: () => string | Promise<string>, logger: ILogger, logMessageContent: boolean);
        connect(url: string, transferFormat: TransferFormat): Promise<void>;
        send(data: any): Promise<void>;
        stop(): Promise<void>;
        private close;
        onreceive: (data: string | ArrayBuffer) => void;
        onclose: (error?: Error) => void;
    }

    /** Defines the expected type for a receiver of results streamed by the server.
     *
     * @typeparam T The type of the items being sent by the server.
     */
    interface IStreamSubscriber<T> {
        /** A boolean that will be set by the {@link @aspnet/signalr.IStreamResult} when the stream is closed. */
        closed?: boolean;
        /** Called by the framework when a new item is available. */
        next(value: T): void;
        /** Called by the framework when an error has occurred.
         *
         * After this method is called, no additional methods on the {@link @aspnet/signalr.IStreamSubscriber} will be called.
         */
        error(err: any): void;
        /** Called by the framework when the end of the stream is reached.
         *
         * After this method is called, no additional methods on the {@link @aspnet/signalr.IStreamSubscriber} will be called.
         */
        complete(): void;
    }

    /** Defines the result of a streaming hub method.
     *
     * @typeparam T The type of the items being sent by the server.
     */
    interface IStreamResult<T> {
        /** Attaches a {@link @aspnet/signalr.IStreamSubscriber}, which will be invoked when new items are available from the stream.
         *
         * @param {IStreamSubscriber<T>} observer The subscriber to attach.
         * @returns {ISubscription<T>} A subscription that can be disposed to terminate the stream and stop calling methods on the {@link @aspnet/signalr.IStreamSubscriber}.
         */
        subscribe(subscriber: IStreamSubscriber<T>): ISubscription<T>;
    }

    /** An interface that allows an {@link @aspnet/signalr.IStreamSubscriber} to be disconnected from a stream.
     *
     * @typeparam T The type of the items being sent by the server.
     */
    interface ISubscription<T> {
        /** Disconnects the {@link @aspnet/signalr.IStreamSubscriber} associated with this subscription from the stream. */
        dispose(): void;
    }

    /** @private */
    class TextMessageFormat {
        static RecordSeparatorCode: number;
        static RecordSeparator: string;
        static write(output: string): string;
        static parse(input: string): string[];
    }

    /** @private */
    class Arg {
        static isRequired(val: any, name: string): void;
        static isIn(val: any, values: any, name: string): void;
    }
    /** @private */

    function getDataDetail(data: any, includeContent: boolean): string;
    /** @private */
    function formatArrayBuffer(data: ArrayBuffer): string;

    /** @private */
    function sendMessage(logger: ILogger, transportName: string, httpClient: HttpClient, url: string, accessTokenFactory: () => string | Promise<string>, content: string | ArrayBuffer, logMessageContent: boolean): Promise<void>;

    /** @private */
    function createLogger(logger?: ILogger | LogLevel): ILogger;

    /** @private */
    class Subject<T> implements IStreamResult<T> {
        observers: Array<IStreamSubscriber<T>>;
        cancelCallback: () => Promise<void>;
        constructor(cancelCallback: () => Promise<void>);
        next(item: T): void;
        error(err: any): void;
        complete(): void;
        subscribe(observer: IStreamSubscriber<T>): ISubscription<T>;
    }

    /** @private */
    class SubjectSubscription<T> implements ISubscription<T> {
        private subject;
        private observer;
        constructor(subject: Subject<T>, observer: IStreamSubscriber<T>);
        dispose(): void;
    }

    /** @private */
    class ConsoleLogger implements ILogger {
        private readonly minimumLogLevel;
        constructor(minimumLogLevel: LogLevel);
        log(logLevel: LogLevel, message: string): void;
    }

    /** @private */
    class WebSocketTransport implements ITransport {
        private readonly logger;
        private readonly accessTokenFactory;
        private readonly logMessageContent;
        private webSocket;
        constructor(accessTokenFactory: () => string | Promise<string>, logger: ILogger, logMessageContent: boolean);
        connect(url: string, transferFormat: TransferFormat): Promise<void>;
        send(data: any): Promise<void>;
        stop(): Promise<void>;
        onreceive: (data: string | ArrayBuffer) => void;
        onclose: (error?: Error) => void;
    }
}