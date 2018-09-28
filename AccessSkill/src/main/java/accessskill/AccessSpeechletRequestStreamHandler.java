package accessskill;

import java.util.HashSet;
import java.util.Set;

import com.amazon.speech.speechlet.Speechlet;
import com.amazon.speech.speechlet.lambda.SpeechletRequestStreamHandler;


public class AccessSpeechletRequestStreamHandler extends SpeechletRequestStreamHandler {

    private static final Set<String> supportedApplicationIds;

    static {
 
        supportedApplicationIds = new HashSet<String>();
        
    }

    public AccessSpeechletRequestStreamHandler() {
        super(new AccessSpeechlet(), supportedApplicationIds);
    }

    public AccessSpeechletRequestStreamHandler(Speechlet speechlet,
            Set<String> supportedApplicationIds) {
        super(speechlet, supportedApplicationIds);
    }

}
